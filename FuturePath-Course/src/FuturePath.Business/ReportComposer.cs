using System;
using System.Collections.Generic;
using System.Text;

namespace FuturePath.Business
{
    public class ReportComposer
    {
        public static string GapTotals(IReadOnlyList<SessionSummary> sessions)
        {
            int subjectData = 0;
            int markGaps = 0;
            int questionnaires = 0;
            foreach (SessionSummary session in sessions)
            {

                string reasons = session.Explanation;
                if (reasons.Contains("Required:", StringComparison.Ordinal))
                {
                    subjectData++;
                }

                if (reasons.Contains("Improve ", StringComparison.Ordinal) || reasons.Contains("Overall average ", StringComparison.Ordinal))
                {
                    markGaps++;
                }

                if (reasons.Contains("Complete the interest questionnaire", StringComparison.Ordinal))
                {
                    questionnaires++;
                }
            }

            return "SESSIONS WITH IDENTIFIED GAPS\n  Missing/unconfirmed required-subject data: " + subjectData + "\n  Mark/average shortfalls: " + markGaps + "\n  Incomplete questionnaires: " + questionnaires + "\n";
        }

        public ReportDocument Summary(AdvisorySession session, IReadOnlyList<FundingOption> funding, DateTime now)
        {
            LearnerProfile learner = session.Input.Learner;
            StringBuilder b = new StringBuilder();
            b.AppendLine("FUTURE PATH  |  LEARNER ADVISORY SUMMARY");
            b.AppendLine($"Generated: {now:u}  •  Session #{session.Id}  •  Assessed: {session.CreatedUtc:u}");
            b.AppendLine("Rules: " + session.RuleVersion);
            b.AppendLine();
            string name = learner.Name.Length == 0 ? "Anonymous learner" : learner.Name;
            b.AppendLine("Learner: " + name + "  |  Grade " + learner.Grade);
            b.AppendLine("School: " + learner.School + "  |  Province: " + learner.Province);
            b.AppendLine("Email: " + learner.Email + "  |  Phone: " + learner.Phone);
            b.AppendLine();
            b.AppendLine("Selected: " + session.Selected.Programme + " (" + session.Selected.Faculty + ")");
            b.AppendLine("Outcome: " + session.Selected.OutcomeText);
            b.AppendLine($"Overall recommendation score: {session.Selected.OverallScore:0.##}%");
            b.AppendLine($"Subject coverage {session.Selected.SubjectScore:0.##}% • mark match {session.Selected.MarkScore:0.##}% • interest {session.Selected.InterestScore:0.##}% • faculty preference {session.Selected.PreferenceScore:0.##}%");
            b.AppendLine();
            b.AppendLine("SUBJECTS AND MARKS");
            decimal total = 0;
            List<SubjectMark> marks = new List<SubjectMark>(session.Input.Marks);
            marks.Sort(CompareMarks);
            foreach (SubjectMark mark in marks)
            {
                b.AppendLine($"  {mark.SubjectName}: {mark.Mark:0.##}%");
                total += mark.Mark;
            }

            if (marks.Count > 0)
            {
                b.AppendLine($"  Average (all captured subjects): {total / marks.Count:0.##}%");
            }

            b.AppendLine("Subject list confirmed complete: " + (session.Input.SubjectsComplete ? "Yes" : "No"));
            b.AppendLine();
            b.AppendLine("INTEREST QUESTIONNAIRE RESPONSES (1–5)");
            foreach (InterestResponse response in session.Input.Responses)
            {
                b.AppendLine("  Question " + response.QuestionId + ": " + response.Score + " / 5");
            }
            b.AppendLine();
            b.AppendLine("INTEREST CATEGORY RESULTS (0–100%)");
            foreach (KeyValuePair<string, decimal> category in session.InterestScores)
            {
                b.AppendLine($"  {category.Key}: {category.Value:0.##}%");
            }

            b.AppendLine();
            b.AppendLine("EXPLANATION AND IMPROVEMENT AREAS");
            b.AppendLine(session.Selected.Explanation);
            b.AppendLine();
            b.AppendLine("ALTERNATIVE PROGRAMMES (eligible or near-match only)");
            foreach (Recommendation alternative in session.Alternatives)
            {
                b.AppendLine($"  {alternative.Programme} — {alternative.OutcomeText} — {alternative.OverallScore:0.##}% (interest {alternative.InterestScore:0.##}%)");
                b.AppendLine(alternative.Explanation);
                b.AppendLine();
            }

            if (session.Alternatives.Count == 0)
            {
                b.AppendLine("No suitable alternatives can be confirmed from the current data. Complete missing information or discuss other pathways with an advisor.");
            }

            AppendFunding(b, session.Input.FundingRequested, funding);
            b.AppendLine();
            b.AppendLine("NEXT STEPS");
            b.AppendLine("Review the gaps above; discuss alternatives with a career advisor. Verify official entry criteria before applying.");
            b.AppendLine();
            b.AppendLine(RecommendationEngine.Disclaimer);
            b.AppendLine("Privacy: this report contains learner information; share only with intended recipients.");
            ReportDocument report = new ReportDocument();
            report.Title = "Learner Advisory Summary";
            report.GeneratedUtc = now;
            report.SessionId = session.Id;
            report.Content = b.ToString();
            return report;
        }

        private static int CompareMarks(SubjectMark first, SubjectMark second)
        {
            return string.Compare(first.SubjectName, second.SubjectName, StringComparison.CurrentCulture);
        }

        private void AppendFunding(StringBuilder b, bool requested, IReadOnlyList<FundingOption> funding)
        {
            b.AppendLine();
            b.AppendLine("FUNDING INFORMATION");
            if (!requested)
            {
                b.AppendLine("Funding information was not requested in this session.");
                return;
            }

            foreach (FundingOption option in funding)
            {
                b.AppendLine($"  {option.Name} | {option.Type} | Closes {option.ClosingDate:yyyy-MM-dd}");
                b.AppendLine("  Eligibility: " + option.Eligibility);
                b.AppendLine("  Documents: " + option.Documents);
                b.AppendLine("  Contact: " + option.Contact);
                b.AppendLine(option.Sample ? "  SAMPLE / VERIFY WITH PROVIDER" : "  Verify current details with provider");
                b.AppendLine();
            }

            if (funding.Count == 0)
            {
                b.AppendLine("No currently open, active funding records are linked to this programme or faculty. This does not establish funding ineligibility.");
            }
        }
    }
}
