using System;
using System.Collections.Generic;

namespace FuturePath.Business
{
    public class RecommendationEngine
    {
        #region Business Rule Constants
        public const decimal SubjectWeight = 0.35m;
        public const decimal MarkWeight = 0.35m;
        public const decimal InterestWeight = 0.25m;
        public const decimal FacultyWeight = 0.05m;
        public const decimal NearMatchGap = 5m;
        public const decimal StrongInterest = 60m;
        public const string RuleVersion = "FP-1.0: 35S/35M/25I/5F; near miss <=5; strong interest >=60";
        public const string Disclaimer = "Preliminary guidance only, not an admission decision or funding award. " + "Programme requirements and funding records are sample data. Verify current requirements, dates and eligibility with the institution or provider. " + "Grade 11 results are provisional. No external AI or online service is used.";
        #endregion
        #region Interest Calculation
        public Dictionary<string, decimal> CalculateInterests(IEnumerable<InterestResponse> responses, IEnumerable<InterestQuestion> questions)
        {
            Dictionary<string, decimal> totals = new Dictionary<string, decimal>();
            Dictionary<string, int> counts = new Dictionary<string, int>();
            foreach (InterestQuestion question in questions)
            {
                foreach (InterestResponse answer in responses)
                {
                    if (answer.QuestionId != question.Id)
                    {
                        continue;
                    }

                    if (!totals.ContainsKey(question.Category))
                    {
                        totals.Add(question.Category, 0);
                        counts.Add(question.Category, 0);
                    }

                    totals[question.Category] += (answer.Score - 1) * 25m;
                    counts[question.Category]++;
                }
            }

            Dictionary<string, decimal> scores = new Dictionary<string, decimal>();
            foreach (string category in totals.Keys)
            {
                scores.Add(category, Math.Round(totals[category] / counts[category], 2));
            }

            return scores;
        }

        private decimal CategoryScore(int categoryId, AdvisoryInput input, IReadOnlyList<InterestQuestion> questions)
        {
            decimal total = 0;
            int count = 0;
            foreach (InterestQuestion question in questions)
            {
                if (question.CategoryId != categoryId)
                {
                    continue;
                }

                foreach (InterestResponse answer in input.Responses)
                {
                    if (answer.QuestionId == question.Id)
                    {
                        total += (answer.Score - 1) * 25m;
                        count++;
                    }
                }
            }

            if (count == 0)
            {
                return 0;
            }

            return total / count;
        }

        private bool HasCategoryQuestion(int categoryId, IReadOnlyList<InterestQuestion> questions)
        {
            foreach (InterestQuestion question in questions)
            {
                if (question.CategoryId == categoryId)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
        #region Subject Requirements
        private SubjectMark? FindMark(int subjectId, List<SubjectMark> marks)
        {
            foreach (SubjectMark mark in marks)
            {
                if (mark.SubjectId == subjectId)
                {
                    return mark;
                }
            }

            return null;
        }

        private List<List<ProgrammeRequirement>> RequiredGroups(DegreeProgramme programme)
        {
            List<List<ProgrammeRequirement>> groups = new List<List<ProgrammeRequirement>>();
            List<List<ProgrammeRequirement>> alternatives = new List<List<ProgrammeRequirement>>();
            foreach (ProgrammeRequirement requirement in programme.Requirements)
            {
                if (!requirement.Mandatory)
                {
                    continue;
                }

                if (requirement.AlternativeGroup == 0)
                {
                    groups.Add(new List<ProgrammeRequirement> { requirement });
                    continue;
                }

                List<ProgrammeRequirement>? match = null;
                foreach (List<ProgrammeRequirement> group in alternatives)
                {
                    if (group[0].AlternativeGroup == requirement.AlternativeGroup)
                    {
                        match = group;
                        break;
                    }
                }

                if (match == null)
                {
                    match = new List<ProgrammeRequirement>();
                    alternatives.Add(match);
                }

                match.Add(requirement);
            }

            foreach (List<ProgrammeRequirement> group in alternatives)
            {
                groups.Add(group);
            }

            return groups;
        }

        private string RequirementLabel(List<ProgrammeRequirement> group)
        {
            List<string> choices = new List<string>();
            foreach (ProgrammeRequirement requirement in group)
            {
                choices.Add(requirement.Subject + " at " + requirement.MinimumMark.ToString("0.##") + "%");
            }

            return string.Join(" OR ", choices);
        }

        #endregion
        #region Check One Degree
        // Each helper performs one part of the assessment; none opens a database.
        private class EligibilityCheck
        {
            public bool Missing;
            public bool HardGap;
            public bool NearGap;
            public decimal SubjectTotal;
            public decimal MarkTotal;
            public List<string> Reasons = new List<string>();
        }

        public Recommendation Evaluate(AdvisoryInput input, DegreeProgramme programme, IReadOnlyList<InterestQuestion> questions)
        {
            EligibilityCheck check = new EligibilityCheck();
            List<List<ProgrammeRequirement>> groups = RequiredGroups(programme);
            CheckConfiguration(programme, questions, groups.Count, check);
            CheckSubjects(input, groups, check);
            CheckAverage(input, programme, check);
            CheckQuestionnaire(input, questions, check);
            decimal interest = CalculateDegreeInterest(input, programme, questions);
            decimal subjectScore = 0;
            decimal markScore = 0;
            if (groups.Count > 0)
            {
                subjectScore = check.SubjectTotal / groups.Count;
                markScore = check.MarkTotal / groups.Count;
            }

            decimal preference = 0;
            if (input.PreferredFacultyId == programme.FacultyId)
            {
                preference = 100;
            }

            string interestMessage = "Interest alignment: " + interest.ToString("0.##") + "%. ";
            if (interest >= StrongInterest)
            {
                interestMessage += "Your interests align with this study area.";
            }
            else
            {
                interestMessage += "Consider alternatives that align more closely with your interests.";
            }

            check.Reasons.Add(interestMessage);
            AddOptionalNotes(input, programme, check);
            Recommendation result = new Recommendation();
            result.DegreeId = programme.Id;
            result.Programme = programme.Name;
            result.Faculty = programme.Faculty;
            result.Outcome = ChooseOutcome(check, interest);
            result.SubjectScore = Math.Round(subjectScore, 2);
            result.MarkScore = Math.Round(markScore, 2);
            result.InterestScore = Math.Round(interest, 2);
            result.PreferenceScore = preference;
            result.OverallScore = Math.Round(subjectScore * SubjectWeight + markScore * MarkWeight + interest * InterestWeight + preference * FacultyWeight, 2);
            result.Explanation = string.Join(Environment.NewLine, check.Reasons);
            return result;
        }

        private void CheckConfiguration(DegreeProgramme programme, IReadOnlyList<InterestQuestion> questions, int groupCount, EligibilityCheck check)
        {
            bool configurationMissing = groupCount == 0 || programme.Interests.Count == 0;
            foreach (InterestWeight mapping in programme.Interests)
            {
                if (!HasCategoryQuestion(mapping.CategoryId, questions))
                {
                    configurationMissing = true;
                }
            }

            if (configurationMissing)
            {
                check.Missing = true;
                check.Reasons.Add("Programme configuration is incomplete; ask an administrator to add requirements and interest mappings.");
            }
        }

        private void CheckSubjects(AdvisoryInput input, List<List<ProgrammeRequirement>> groups, EligibilityCheck check)
        {
            foreach (List<ProgrammeRequirement> group in groups)
            {
                string label = RequirementLabel(group);
                ProgrammeRequirement? best = null;
                SubjectMark? bestMark = null;
                foreach (ProgrammeRequirement requirement in group)
                {
                    SubjectMark? mark = FindMark(requirement.SubjectId, input.Marks);
                    if (mark == null)
                    {
                        continue;
                    }

                    if (best == null || mark.Mark - requirement.MinimumMark > bestMark!.Mark - best.MinimumMark)
                    {
                        best = requirement;
                        bestMark = mark;
                    }
                }

                if (best == null || bestMark == null)
                {
                    string message = "Required: " + label + ". No mark was supplied. ";
                    if (input.SubjectsComplete)
                    {
                        check.HardGap = true;
                        message += "This required subject is missing from your confirmed subject list.";
                    }
                    else
                    {
                        check.Missing = true;
                        message += "Complete your subject list to confirm this requirement.";
                    }

                    check.Reasons.Add(message);
                    continue;
                }

                check.SubjectTotal += 100;
                if (best.MinimumMark == 0)
                {
                    check.MarkTotal += 100;
                }
                else
                {
                    check.MarkTotal += Math.Min(100, bestMark.Mark / best.MinimumMark * 100);
                }

                decimal gap = best.MinimumMark - bestMark.Mark;
                if (gap > 0)
                {
                    string message = "Improve " + best.Subject + ": " + bestMark.Mark.ToString("0.##") + "% entered; " + best.MinimumMark.ToString("0.##") + "% required (gap " + gap.ToString("0.##") + " percentage points).";
                    if (group.Count > 1)
                    {
                        message += " Accepted choices: " + label + ".";
                    }

                    check.Reasons.Add(message);
                    if (gap <= NearMatchGap)
                    {
                        check.NearGap = true;
                    }
                    else
                    {
                        check.HardGap = true;
                    }
                }
                else
                {
                    check.Reasons.Add("Met: " + best.Subject + " " + bestMark.Mark.ToString("0.##") + "% (minimum " + best.MinimumMark.ToString("0.##") + "%).");
                }
            }

            if (!input.SubjectsComplete)
            {
                check.Missing = true;
                check.Reasons.Add("Confirm that all current subjects and marks have been captured.");
            }
        }

        private void CheckAverage(AdvisoryInput input, DegreeProgramme programme, EligibilityCheck check)
        {
            if (programme.MinimumAverage > 0 && input.Marks.Count > 0)
            {
                decimal total = 0;
                foreach (SubjectMark mark in input.Marks)
                {
                    total += mark.Mark;
                }

                decimal average = total / input.Marks.Count;
                decimal gap = programme.MinimumAverage - average;
                if (gap > 0)
                {
                    check.Reasons.Add("Overall average " + average.ToString("0.##") + "% is below " + programme.MinimumAverage.ToString("0.##") + "% by " + gap.ToString("0.##") + " points (all captured subjects included).");
                    if (gap <= NearMatchGap)
                    {
                        check.NearGap = true;
                    }
                    else
                    {
                        check.HardGap = true;
                    }
                }
            }
        }

        private void CheckQuestionnaire(AdvisoryInput input, IReadOnlyList<InterestQuestion> questions, EligibilityCheck check)
        {
            int unanswered = 0;
            foreach (InterestQuestion question in questions)
            {
                bool answered = false;
                foreach (InterestResponse response in input.Responses)
                {
                    if (response.QuestionId == question.Id)
                    {
                        answered = true;
                        break;
                    }
                }

                if (!answered)
                {
                    unanswered++;
                }
            }

            if (questions.Count == 0 || unanswered > 0)
            {
                check.Missing = true;
                check.Reasons.Add("Complete the interest questionnaire (" + unanswered + " unanswered active questions).");
            }
        }

        private decimal CalculateDegreeInterest(AdvisoryInput input, DegreeProgramme programme, IReadOnlyList<InterestQuestion> questions)
        {
            decimal weightedInterest = 0;
            decimal totalWeight = 0;
            foreach (InterestWeight mapping in programme.Interests)
            {
                weightedInterest += CategoryScore(mapping.CategoryId, input, questions) * mapping.Weight;
                totalWeight += mapping.Weight;
            }

            decimal interest = 0;
            if (totalWeight > 0)
            {
                interest = weightedInterest / totalWeight;
            }

            return interest;
        }

        private void AddOptionalNotes(AdvisoryInput input, DegreeProgramme programme, EligibilityCheck check)
        {
            foreach (ProgrammeRequirement requirement in programme.Requirements)
            {
                if (requirement.Mandatory)
                {
                    continue;
                }

                SubjectMark? mark = FindMark(requirement.SubjectId, input.Marks);
                if (mark == null || mark.Mark < requirement.MinimumMark)
                {
                    check.Reasons.Add("Helpful, not compulsory: " + requirement.Subject + " at " + requirement.MinimumMark.ToString("0.##") + "%. " + requirement.Note);
                }
            }
        }

        private AdvisoryOutcome ChooseOutcome(EligibilityCheck check, decimal interest)
        {
            if (check.Missing)
            {
                return AdvisoryOutcome.MoreInformationNeeded;
            }

            if (check.HardGap)
            {
                return AdvisoryOutcome.NotCurrentlyEligible;
            }

            if (check.NearGap || interest < StrongInterest)
            {
                return AdvisoryOutcome.PossibleMatch;
            }

            return AdvisoryOutcome.StrongMatch;
        }

        #endregion
        #region Alternatives and Complete Assessment
        public List<Recommendation> GenerateAlternatives(AdvisoryInput input, IReadOnlyList<DegreeProgramme> programmes, IReadOnlyList<InterestQuestion> questions)
        {
            List<Recommendation> ranked = new List<Recommendation>();
            foreach (DegreeProgramme programme in programmes)
            {
                if (!programme.Active || programme.Id == input.DegreeId)
                {
                    continue;
                }

                Recommendation candidate = Evaluate(input, programme, questions);
                if (candidate.Outcome != AdvisoryOutcome.StrongMatch && candidate.Outcome != AdvisoryOutcome.PossibleMatch)
                {
                    continue;
                }

                int index = 0;
                while (index < ranked.Count && Compare(ranked[index], candidate) <= 0)
                {
                    index++;
                }

                ranked.Insert(index, candidate);
                if (ranked.Count > 5)
                {
                    ranked.RemoveAt(5);
                }
            }

            return ranked;
        }

        private int Compare(Recommendation first, Recommendation second)
        {
            int firstRank = first.Outcome == AdvisoryOutcome.StrongMatch ? 0 : 1;
            int secondRank = second.Outcome == AdvisoryOutcome.StrongMatch ? 0 : 1;
            if (firstRank != secondRank)
            {
                return firstRank.CompareTo(secondRank);
            }

            int difference = second.OverallScore.CompareTo(first.OverallScore);
            if (difference != 0)
            {
                return difference;
            }

            difference = string.Compare(first.Programme, second.Programme, StringComparison.Ordinal);
            if (difference != 0)
            {
                return difference;
            }

            return first.DegreeId.CompareTo(second.DegreeId);
        }

        public AdvisorySession Assess(AdvisoryInput input, IReadOnlyList<DegreeProgramme> programmes, IReadOnlyList<InterestQuestion> questions, DateTime now)
        {
            DegreeProgramme? selected = null;
            foreach (DegreeProgramme programme in programmes)
            {
                if (programme.Id == input.DegreeId && programme.Active)
                {
                    selected = programme;
                    break;
                }
            }

            if (selected == null)
            {
                throw new ValidationException("Select an active degree programme.");
            }

            AdvisorySession session = new AdvisorySession();
            session.CreatedUtc = now;
            session.Input = input;
            session.Selected = Evaluate(input, selected, questions);
            session.Alternatives = GenerateAlternatives(input, programmes, questions);
            session.InterestScores = CalculateInterests(input.Responses, questions);
            return session;
        }
        #endregion
    }
}
