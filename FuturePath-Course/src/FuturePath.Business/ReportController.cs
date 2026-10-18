using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using System.Text;

namespace FuturePath.Business
{
    public sealed class ReportController
    {
        private readonly IFuturePathStore store;
        private readonly AccessSession access;
        private readonly IClock clock;
        public ReportController(IFuturePathStore store, AccessSession access, IClock clock)
        {
            this.store = store;
            this.access = access;
            this.clock = clock;
        }

        public ReportDocument Summary(int sessionId)
        {
            access.RequireSession(sessionId);
            AdvisorySession session = store.GetSession(sessionId) ?? throw new ValidationException("Session not found.");
            List<FundingOption> funding = new List<FundingOption>();
            if (session.Input.FundingRequested)
            {
                funding = store.GetFunding(session.Selected.DegreeId, clock.Today);
            }

            ReportComposer composer = new ReportComposer();
            return composer.Summary(session, funding, clock.UtcNow);
        }

        public ReportDocument Activity(DateOnly from, DateOnly to, bool exceptionsOnly, string search = "", int facultyId = 0, int degreeId = 0, string outcome = "")
        {
            access.RequireStaff();
            if (from > to)
            {
                throw new ValidationException("The start date must be on or before the end date.");
            }

            List<SessionSummary> rows = new List<SessionSummary>();
            HashSet<int> learners = new HashSet<int>();
            foreach (SessionSummary session in store.FindSessions(from, to, search.Trim()))
            {
                if (facultyId > 0 && session.FacultyId != facultyId)
                {
                    continue;
                }

                if (degreeId > 0 && session.DegreeId != degreeId)
                {
                    continue;
                }

                if (outcome.Length > 0 && session.Outcome != outcome)
                {
                    continue;
                }

                bool exception = session.Outcome == "Not Currently Eligible" || session.Outcome == "More Information Needed";
                if (!exceptionsOnly || exception)
                {
                    rows.Add(session);
                    learners.Add(session.LearnerId);
                }
            }

            string title = exceptionsOnly ? "Learners Not Currently Eligible — Exception Report" : "Degree Interest and Eligibility Report";
            StringBuilder b = new StringBuilder();
            b.AppendLine("FUTURE PATH  |  " + title.ToUpperInvariant());
            b.AppendLine($"Generated: {clock.UtcNow:u}");
            b.AppendLine($"Session dates: {from:yyyy-MM-dd} to {to:yyyy-MM-dd} (UTC, inclusive). Search: {(search.Length == 0 ? "All learners/programmes" : search)}");
            b.AppendLine("Faculty filter: " + FacultyName(facultyId) + " | Degree filter: " + DegreeName(degreeId) + " | Outcome: " + (outcome.Length == 0 ? "All" : outcome));
            b.AppendLine("Sessions: " + rows.Count + "  |  Distinct learners: " + learners.Count);
            b.AppendLine();
            if (!exceptionsOnly)
            {
                AppendTotals(b, rows);
            }

            b.AppendLine();
            b.AppendLine(exceptionsOnly ? "LEARNERS REQUIRING FOLLOW-UP" : "SESSION DETAILS");
            rows.Sort(exceptionsOnly ? CompareExceptions : CompareSessions);
            foreach (SessionSummary session in rows)
            {
                b.AppendLine($"#{session.Id} | {session.Learner} | Grade {session.Grade} | {session.CreatedUtc:yyyy-MM-dd} | {session.Programme}");
                b.AppendLine($"  {session.Outcome} — {session.Score:0.##}%");
                b.AppendLine("  Faculty: " + session.Faculty);
                b.AppendLine(session.Explanation);
                if (exceptionsOnly)
                {
                    AdvisorySession detail = store.GetSession(session.Id) ?? throw new ValidationException("Session not found.");
                    b.AppendLine("  Interest alignment: " + detail.Selected.InterestScore.ToString("0.##") + "%");
                    b.AppendLine("  Alternative degree recommendations:");
                    foreach (Recommendation alternative in detail.Alternatives)
                    {
                        b.AppendLine("    " + alternative.Programme + " — " + alternative.OutcomeText + " — " + alternative.OverallScore.ToString("0.##") + "%");
                        b.AppendLine(alternative.Explanation);
                    }
                    if (detail.Alternatives.Count == 0)
                    {
                        b.AppendLine("    No eligible or near-match alternatives can be confirmed.");
                    }
                }
                b.AppendLine();
            }

            if (rows.Count == 0)
            {
                b.AppendLine("No sessions match these filters.");
            }

            b.AppendLine("Counts describe advisory sessions, not admission decisions. A learner can have multiple sessions.");
            b.AppendLine(RecommendationEngine.Disclaimer);
            b.AppendLine("Staff use only. Protect learner privacy.");
            ReportDocument report = new ReportDocument();
            report.Title = title;
            report.GeneratedUtc = clock.UtcNow;
            report.Content = b.ToString();
            return report;
        }

        private static int CompareExceptions(SessionSummary first, SessionSummary second)
        {
            int firstOrder = first.Outcome == "Not Currently Eligible" ? 0 : 1;
            int secondOrder = second.Outcome == "Not Currently Eligible" ? 0 : 1;
            int difference = firstOrder.CompareTo(secondOrder);
            return difference == 0 ? CompareSessions(first, second) : difference;
        }

        private static int CompareSessions(SessionSummary first, SessionSummary second)
        {
            int difference = string.Compare(first.Learner, second.Learner, StringComparison.CurrentCulture);
            if (difference != 0)
            {
                return difference;
            }

            return first.CreatedUtc.CompareTo(second.CreatedUtc);
        }

        private int OutcomeCount(List<SessionSummary> rows, string outcome)
        {
            int count = 0;
            foreach (SessionSummary session in rows)
            {
                if (session.Outcome == outcome)
                {
                    count++;
                }
            }

            return count;
        }

        private void AddCount(Dictionary<string, int> counts, string name)
        {
            if (!counts.ContainsKey(name))
            {
                counts.Add(name, 0);
            }

            counts[name]++;
        }

        private List<string> RankedNames(Dictionary<string, int> counts)
        {
            List<string> ranked = new List<string>();
            foreach (string name in counts.Keys)
            {
                int index = 0;
                while (index < ranked.Count)
                {
                    string previous = ranked[index];
                    if (counts[name] > counts[previous])
                    {
                        break;
                    }

                    if (counts[name] == counts[previous] && string.Compare(name, previous, StringComparison.CurrentCulture) < 0)
                    {
                        break;
                    }

                    index++;
                }

                ranked.Insert(index, name);
            }

            return ranked;
        }

        private void AppendTotals(StringBuilder b, List<SessionSummary> rows)
        {
            b.AppendLine("OUTCOMES");
            string[] outcomes =
            {
                "Strong Match",
                "Possible Match",
                "Not Currently Eligible",
                "More Information Needed"
            };
            foreach (string outcome in outcomes)
            {
                b.AppendLine("  " + outcome + ": " + OutcomeCount(rows, outcome));
            }

            Dictionary<string, int> faculties = new Dictionary<string, int>();
            Dictionary<string, int> degrees = new Dictionary<string, int>();
            int funding = 0;
            foreach (SessionSummary session in rows)
            {
                AddCount(faculties, session.Faculty);
                AddCount(degrees, session.Programme);
                if (session.FundingRequested)
                {
                    funding++;
                }
            }

            b.AppendLine();
            b.AppendLine("MOST SELECTED FACULTIES");
            foreach (string faculty in RankedNames(faculties))
            {
                b.AppendLine("  " + faculty + ": " + faculties[faculty] + " sessions");
            }

            b.AppendLine();
            b.AppendLine("SELECTED DEGREES / OUTCOMES");
            foreach (string degree in RankedNames(degrees))
            {
                List<SessionSummary> selected = new List<SessionSummary>();
                foreach (SessionSummary session in rows)
                {
                    if (session.Programme == degree)
                    {
                        selected.Add(session);
                    }
                }

                b.AppendLine($"  {degree}: {selected.Count} sessions | strong {OutcomeCount(selected, "Strong Match")} | possible {OutcomeCount(selected, "Possible Match")} | not eligible {OutcomeCount(selected, "Not Currently Eligible")} | incomplete {OutcomeCount(selected, "More Information Needed")}");
            }

            b.AppendLine();
            b.AppendLine(ReportComposer.GapTotals(rows));
            b.AppendLine();
            AppendCommonGaps(b, rows);
            b.AppendLine("Funding requested: " + funding + " of " + rows.Count + " sessions");
            AppendFundingCategories(b, rows);
        }

        private string FacultyName(int id)
        {
            if (id == 0)
            {
                return "All";
            }

            foreach (LookupItem faculty in store.GetLookup("Faculty:all"))
            {
                if (faculty.Id == id)
                {
                    return faculty.Name;
                }
            }

            return "Unknown faculty";
        }

        private string DegreeName(int id)
        {
            if (id == 0)
            {
                return "All";
            }

            foreach (DegreeProgramme degree in store.GetProgrammes(false))
            {
                if (degree.Id == id)
                {
                    return degree.Name;
                }
            }

            return "Unknown degree";
        }

        public List<LookupItem> Faculties()
        {
            access.RequireStaff();
            return store.GetLookup("Faculty:all");
        }

        public List<DegreeProgramme> Programmes()
        {
            access.RequireStaff();
            return store.GetProgrammes(false);
        }

        private void AppendCommonGaps(StringBuilder b, List<SessionSummary> rows)
        {
            Dictionary<string, int> subjects = new Dictionary<string, int>();
            Dictionary<string, int> marks = new Dictionary<string, int>();
            foreach (SessionSummary session in rows)
            {
                string[] lines = session.Explanation.Replace("\r\n", "\n").Split('\n');
                foreach (string line in lines)
                {
                    if (line.StartsWith("Required: ", StringComparison.Ordinal))
                    {
                        int end = line.IndexOf(". No mark was supplied.", StringComparison.Ordinal);
                        if (end > 10)
                        {
                            AddCount(subjects, line.Substring(10, end - 10));
                        }
                    }

                    if (line.StartsWith("Improve ", StringComparison.Ordinal))
                    {
                        int end = line.IndexOf(':');
                        if (end > 8)
                        {
                            AddCount(marks, line.Substring(8, end - 8));
                        }
                    }

                    if (line.StartsWith("Overall average ", StringComparison.Ordinal))
                    {
                        AddCount(marks, "Overall average");
                    }
                }
            }

            b.AppendLine("COMMON SUBJECT GAPS (missing/unconfirmed requirement)");
            foreach (string name in RankedNames(subjects))
            {
                b.AppendLine("  " + name + ": " + subjects[name]);
            }

            b.AppendLine("COMMON MARK GAPS");
            foreach (string name in RankedNames(marks))
            {
                b.AppendLine("  " + name + ": " + marks[name]);
            }
        }

        private void AppendFundingCategories(StringBuilder b, List<SessionSummary> rows)
        {
            Dictionary<string, int> categories = new Dictionary<string, int>();
            foreach (SessionSummary session in rows)
            {
                if (!session.FundingRequested)
                {
                    continue;
                }

                HashSet<string> shown = new HashSet<string>();
                foreach (FundingOption option in store.GetFunding(session.DegreeId, clock.Today))
                {
                    shown.Add(option.Type);
                }

                foreach (string type in shown)
                {
                    AddCount(categories, type);
                }

                if (shown.Count == 0)
                {
                    AddCount(categories, "No currently open linked category");
                }
            }

            b.AppendLine("FUNDING REQUESTS BY LINKED CATEGORY");
            b.AppendLine("  A funding opt-in requests all linked guidance. Each session counts once per currently open category; totals can exceed the number of requests. Current reference data is used, not an award or application history.");
            foreach (string type in RankedNames(categories))
            {
                b.AppendLine("  " + type + ": " + categories[type]);
            }
        }

        public void Export(ReportDocument report, string path)
        {
            if (report.SessionId is int id)
            {
                access.RequireSession(id);
            }
            else
            {
                access.RequireStaff();
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ValidationException("Choose an export file.");
            }

            string content = Path.GetExtension(path).Equals(".html", StringComparison.OrdinalIgnoreCase) ? report.ToHtml() : report.Content;

            string temp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                File.WriteAllText(temp, content, new UTF8Encoding(false));
                File.Move(temp, path, true);
            }
            finally
            {
                if (File.Exists(temp))
                {
                    File.Delete(temp);
                }
            }

            if (report.SessionId is int sessionId)
            {
                store.RecordExport(sessionId, path, clock.UtcNow);
            }

            store.Audit(access.UserId, "Report exported", report.Title, clock.UtcNow);
        }
    }
}
