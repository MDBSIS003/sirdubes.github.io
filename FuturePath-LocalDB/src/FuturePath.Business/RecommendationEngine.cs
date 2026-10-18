namespace FuturePath.Business;
/// <summary>Deterministic local rules. Mandatory gaps always override a high ranking score.</summary>
public sealed class RecommendationEngine
{
    public const decimal SubjectWeight = 0.35m;
    public const decimal MarkWeight = 0.35m;
    public const decimal InterestWeight = 0.25m;
    public const decimal FacultyWeight = 0.05m;
    public const decimal NearMatchGap = 5m;
    public const decimal StrongInterest = 60m;
    public const string RuleVersion = "FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60";
    public const string Disclaimer = "Preliminary guidance only, not an admission decision or funding award. " + "Programme requirements and funding records are sample data. Verify current requirements, dates and eligibility with the institution or provider. " + "Grade 11 results are provisional. No external AI or online service is used.";
    public Dictionary<string, decimal> CalculateInterests(IEnumerable<InterestResponse> responses, IEnumerable<InterestQuestion> questions)
    {
        var answers = responses.ToDictionary(r => r.QuestionId, r => r.Score);
        return questions.Where(q => answers.ContainsKey(q.Id)).GroupBy(q => q.Category).ToDictionary(g => g.Key, g => Math.Round(g.Average(q => (answers[q.Id] - 1) * 25m), 2));
    }

    public Recommendation Evaluate(AdvisoryInput input, DegreeProgramme programme, IReadOnlyList<InterestQuestion> questions)
    {
        var reasons = new List<string>();
        var missing = false;
        var hardGap = false;
        var nearGap = false;
        var groups = RequiredGroups(programme);
        if (groups.Count == 0 || programme.Interests.Count == 0 || programme.Interests.Any(w => !questions.Any(q => q.CategoryId == w.CategoryId)))
        {
            missing = true;
            reasons.Add("Programme configuration is incomplete; ask an administrator to add requirements and interest mappings.");
        }

        var marks = input.Marks.ToDictionary(m => m.SubjectId, m => m.Mark);
        decimal subjectTotal = 0, markTotal = 0;
        foreach (var group in groups)
        {
            var present = group.Where(r => marks.ContainsKey(r.SubjectId)).ToList();
            var label = string.Join(" OR ", group.Select(r => $"{r.Subject} at {r.MinimumMark:0.##}%"));
            if (present.Count == 0)
            {
                reasons.Add($"Required: {label}. No mark was supplied. " + (input.SubjectsComplete ? "This required subject is missing from your confirmed subject list." : "Complete your subject list to confirm this requirement."));
                if (input.SubjectsComplete)
                    hardGap = true;
                else
                    missing = true;
                continue;
            }

            subjectTotal += 100;
            var best = present.OrderByDescending(r => marks[r.SubjectId] - r.MinimumMark).First();
            var mark = marks[best.SubjectId];
            markTotal += best.MinimumMark == 0 ? 100 : Math.Min(100, mark / best.MinimumMark * 100);
            var gap = best.MinimumMark - mark;
            if (gap > 0)
            {
                reasons.Add($"Improve {best.Subject}: {mark:0.##}% entered; {best.MinimumMark:0.##}% required (gap {gap:0.##} percentage points)." + (group.Count > 1 ? $" Accepted choices: {label}." : ""));
                if (gap <= NearMatchGap)
                    nearGap = true;
                else
                    hardGap = true;
            }
            else
                reasons.Add($"Met: {best.Subject} {mark:0.##}% (minimum {best.MinimumMark:0.##}%).");
        }

        if (!input.SubjectsComplete)
        {
            missing = true;
            reasons.Add("Confirm that all current subjects and marks have been captured.");
        }

        if (programme.MinimumAverage > 0 && input.Marks.Count > 0)
        {
            var average = input.Marks.Average(m => m.Mark);
            var gap = programme.MinimumAverage - average;
            if (gap > 0)
            {
                reasons.Add($"Overall average {average:0.##}% is below {programme.MinimumAverage:0.##}% by {gap:0.##} points (all captured subjects included).");
                if (gap <= NearMatchGap)
                    nearGap = true;
                else
                    hardGap = true;
            }
        }

        var scores = input.Responses.ToDictionary(r => r.QuestionId, r => r.Score);
        var unanswered = questions.Count(q => !scores.ContainsKey(q.Id));
        if (questions.Count < 1 || unanswered > 0)
        {
            missing = true;
            reasons.Add($"Complete the interest questionnaire ({unanswered} unanswered active questions).");
        }

        var categories = questions.Where(q => scores.ContainsKey(q.Id)).GroupBy(q => q.CategoryId).ToDictionary(g => g.Key, g => g.Average(q => (scores[q.Id] - 1) * 25m));
        var weights = programme.Interests.Sum(w => w.Weight);
        var interest = weights == 0 ? 0 : programme.Interests.Sum(w => categories.GetValueOrDefault(w.CategoryId) * w.Weight) / weights;
        var subjectScore = groups.Count == 0 ? 0 : subjectTotal / groups.Count;
        var markScore = groups.Count == 0 ? 0 : markTotal / groups.Count;
        var preference = input.PreferredFacultyId == programme.FacultyId ? 100m : 0m;
        reasons.Add($"Interest alignment: {interest:0.##}%. " + (interest >= StrongInterest ? "Your interests align with this study area." : "Consider alternatives that align more closely with your interests."));
        foreach (var optional in programme.Requirements.Where(r => !r.Mandatory))
            if (!marks.TryGetValue(optional.SubjectId, out var mark) || mark < optional.MinimumMark)
                reasons.Add($"Helpful, not compulsory: {optional.Subject} at {optional.MinimumMark:0.##}%. {optional.Note}");
        var outcome = missing ? AdvisoryOutcome.MoreInformationNeeded : hardGap ? AdvisoryOutcome.NotCurrentlyEligible : nearGap || interest < StrongInterest ? AdvisoryOutcome.PossibleMatch : AdvisoryOutcome.StrongMatch;
        return new Recommendation
        {
            DegreeId = programme.Id,
            Programme = programme.Name,
            Faculty = programme.Faculty,
            Outcome = outcome,
            SubjectScore = Math.Round(subjectScore, 2),
            MarkScore = Math.Round(markScore, 2),
            InterestScore = Math.Round(interest, 2),
            PreferenceScore = preference,
            OverallScore = Math.Round(subjectScore * SubjectWeight + markScore * MarkWeight + interest * InterestWeight + preference * FacultyWeight, 2),
            Explanation = string.Join(Environment.NewLine, reasons)
        };
    }

    private static List<List<ProgrammeRequirement>> RequiredGroups(DegreeProgramme programme)
    {
        var groups = new List<List<ProgrammeRequirement>>();
        var alternatives = new Dictionary<int, List<ProgrammeRequirement>>();
        foreach (var requirement in programme.Requirements)
        {
            if (!requirement.Mandatory)
                continue;
            if (requirement.AlternativeGroup == 0)
            {
                groups.Add(new List<ProgrammeRequirement> { requirement });
            }
            else
            {
                if (!alternatives.TryGetValue(requirement.AlternativeGroup, out var group))
                {
                    group = new List<ProgrammeRequirement>();
                    alternatives.Add(requirement.AlternativeGroup, group);
                }

                group.Add(requirement);
            }
        }

        foreach (var group in alternatives.Values)
            groups.Add(group);
        return groups;
    }

    public AdvisorySession Assess(AdvisoryInput input, IReadOnlyList<DegreeProgramme> programmes, IReadOnlyList<InterestQuestion> questions, DateTime now)
    {
        var selected = programmes.SingleOrDefault(p => p.Id == input.DegreeId && p.Active) ?? throw new ValidationException("Select an active degree programme.");
        var result = Evaluate(input, selected, questions);
        var alternatives = programmes.Where(p => p.Active && p.Id != selected.Id).Select(p => Evaluate(input, p, questions)).Where(r => r.Outcome is AdvisoryOutcome.StrongMatch or AdvisoryOutcome.PossibleMatch).OrderBy(r => r.Outcome == AdvisoryOutcome.StrongMatch ? 0 : 1).ThenByDescending(r => r.OverallScore).ThenBy(r => r.Programme, StringComparer.Ordinal).ThenBy(r => r.DegreeId).Take(5).ToList();
        return new AdvisorySession
        {
            CreatedUtc = now,
            Input = input,
            Selected = result,
            Alternatives = alternatives,
            InterestScores = CalculateInterests(input.Responses, questions)
        };
    }
}
