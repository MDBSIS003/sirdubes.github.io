using FuturePathStudentAdvisingSystem.Models;

namespace FuturePathStudentAdvisingSystem.BusinessLogic;

/// <summary>Pure local rules; does not access SQL or user interface controls.</summary>
public class RecommendationService
{
    public List<Recommendation> Rank(AdvisingRecord session, IReadOnlyList<DegreeProgramme> degrees,
        IReadOnlyList<ProgrammeRequirement> requirements, IReadOnlyList<InterestQuestion> questions,
        IReadOnlyList<InterestMapping> mappings)
    {
        if (session.Marks.Count > 0) Validation.Marks(session.Marks);
        var answers = session.Responses.ToDictionary(x => x.QuestionID, x => x.Score);
        var categories = questions.Where(q => answers.ContainsKey(q.QuestionID)).GroupBy(q => q.CategoryID)
            .ToDictionary(g => g.Key, g => g.Average(q => (answers[q.QuestionID] - 1m) * 25m));
        bool questionnaireComplete = questions.Count > 0 && questions.All(q => answers.ContainsKey(q.QuestionID));
        var results = new List<Recommendation>();
        foreach (var degree in degrees.Where(x => x.IsActive))
        {
            var rules = requirements.Where(x => x.DegreeID == degree.DegreeID).ToList();
            // Each named alternative group is ONE required choice. Optional recommendations are not admission gates.
            var gates = rules.Where(x => x.IsMandatory && string.IsNullOrEmpty(x.AlternativeGroup))
                .Select(x => new List<ProgrammeRequirement> { x }).ToList();
            gates.AddRange(rules.Where(x => !string.IsNullOrEmpty(x.AlternativeGroup))
                .GroupBy(x => x.AlternativeGroup).Select(g => g.ToList()));
            var weights = mappings.Where(x => x.DegreeID == degree.DegreeID && x.Weight > 0).ToList();
            bool missing = !session.SubjectsComplete || !questionnaireComplete || gates.Count == 0 || weights.Count == 0 ||
                weights.Any(x => !categories.ContainsKey(x.CategoryID));
            bool absent = false, below = false;
            decimal subjectTotal = 0, markTotal = 0;
            var explanations = new List<string>();
            foreach (var gate in gates)
            {
                var captured = gate.Select(r => new { Rule = r, Mark = session.Marks.FirstOrDefault(m => m.SubjectID == r.SubjectID) })
                    .Where(x => x.Mark != null).ToList();
                if (captured.Count == 0)
                {
                    absent = true;
                    explanations.Add("Missing required subject: " + string.Join(" OR ", gate.Select(r => $"{r.SubjectName} >= {r.MinimumMark:0.##}%")) + ".");
                    continue;
                }
                subjectTotal += 100;
                var best = captured.OrderByDescending(x => x.Rule.MinimumMark == 0 ? 1 : Math.Min(1m, x.Mark!.Mark / x.Rule.MinimumMark)).First();
                decimal ratio = best.Rule.MinimumMark == 0 ? 1m : Math.Min(1m, best.Mark!.Mark / best.Rule.MinimumMark);
                markTotal += ratio * 100;
                if (ratio < 1)
                {
                    below = true;
                    explanations.Add($"{best.Rule.SubjectName}: {best.Mark!.Mark:0.##}%; minimum {best.Rule.MinimumMark:0.##}%; gap {best.Rule.MinimumMark - best.Mark.Mark:0.##} percentage points.");
                }
                else explanations.Add($"{best.Rule.SubjectName}: {best.Mark!.Mark:0.##}% meets the stored {best.Rule.MinimumMark:0.##}% minimum.");
            }
            decimal subject = gates.Count == 0 ? 0 : subjectTotal / gates.Count;
            decimal mark = gates.Count == 0 ? 0 : markTotal / gates.Count;
            decimal interest = weights.Count == 0 ? 0 : weights.Sum(w => categories.GetValueOrDefault(w.CategoryID) * w.Weight) / weights.Sum(w => w.Weight);
            decimal preference = session.FacultyID == degree.FacultyID ? 100 : 0;
            decimal score = decimal.Round(.30m * subject + .30m * mark + .30m * interest + .10m * preference, 2);
            string academic = missing ? "Incomplete" : absent || below ? "Not eligible" : "Eligible";
            // Possible Match is an advisory improvement outcome, never an assertion that failed minima are satisfied.
            string outcome = missing ? "More Information Needed" : absent ? "Not Currently Eligible" : below || interest < 60 ? "Possible Match" : "Strong Match";
            if (missing) explanations.Insert(0, "Complete and confirm subjects, all questionnaire answers, and configured programme rules/mappings before relying on this assessment.");
            explanations.Add($"Interest alignment {interest:0.##}%. Academic status: {academic}. " +
                "Score = 30% subjects + 30% minimum-mark attainment + 30% interest + 10% faculty preference. Sample guidance; acceptance is not guaranteed.");
            results.Add(new(degree.DegreeID, degree.ProgrammeName, degree.FacultyName, academic, outcome,
                decimal.Round(subject, 2), decimal.Round(mark, 2), decimal.Round(interest, 2), score,
                string.Join(" ", explanations), degree.DegreeID != session.DegreeID));
        }
        return results.OrderBy(r => r.AcademicEligibility == "Eligible" ? 0 : r.Outcome == "Possible Match" ? 1 : r.Outcome == "More Information Needed" ? 2 : 3)
            .ThenByDescending(r => r.OverallScore).ThenBy(r => r.ProgrammeName).ToList();
    }
}
