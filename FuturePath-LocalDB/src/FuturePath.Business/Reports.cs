using System.Net;
using System.Text;

namespace FuturePath.Business;
public sealed class ReportDocument
{
    public string Title { get; init; } = "";
    public DateTime GeneratedUtc { get; init; }
    public int? SessionId { get; init; }
    public string Content { get; init; } = "";

    public string ToHtml() => "<!doctype html><html lang=\"en\"><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width\">" + $"<title>{WebUtility.HtmlEncode(Title)}</title><style>body{{font:16px Segoe UI,Arial,sans-serif;color:#172f40;max-width:1000px;margin:40px auto;padding:24px}}" + "h1{color:#11695c}pre{white-space:pre-wrap;font:14px/1.65 Consolas,monospace;border-top:3px solid #168b76;padding-top:24px}" + "@media print{body{margin:0;padding:0}pre{font-size:10pt}}</style>" + $"<h1>{WebUtility.HtmlEncode(Title)}</h1><pre>{WebUtility.HtmlEncode(Content)}</pre></html>";
}

public sealed class ReportComposer
{
    public static string GapTotals(IReadOnlyList<SessionSummary> sessions)
    {
        int subjectData = 0;
        int markGaps = 0;
        int questionnaires = 0;
        foreach (var session in sessions)
        {
            // Count the reasons stored with the assessment; do not recalculate history using today's rules.
            string reasons = session.Explanation;
            if (reasons.Contains("Required:", StringComparison.Ordinal))
                subjectData++;
            if (reasons.Contains("Improve ", StringComparison.Ordinal) || reasons.Contains("Overall average ", StringComparison.Ordinal))
                markGaps++;
            if (reasons.Contains("Complete the interest questionnaire", StringComparison.Ordinal))
                questionnaires++;
        }

        return $"SESSIONS WITH IDENTIFIED GAPS\n  Missing/unconfirmed required-subject data: {subjectData}\n  Mark/average shortfalls: {markGaps}\n  Incomplete questionnaires: {questionnaires}\n";
    }

    public ReportDocument Summary(AdvisorySession session, IReadOnlyList<FundingOption> funding, DateTime now)
    {
        var learner = session.Input.Learner;
        var b = new StringBuilder();
        b.AppendLine("FUTURE PATH  |  LEARNER ADVISORY SUMMARY").AppendLine($"Generated: {now:u}  •  Session #{session.Id}  •  Assessed: {session.CreatedUtc:u}").AppendLine($"Rules: {session.RuleVersion}").AppendLine().AppendLine($"Learner: {(learner.Name.Length == 0 ? "Anonymous learner" : learner.Name)}  |  Grade {learner.Grade}").AppendLine($"School: {learner.School}  |  Province: {learner.Province}").AppendLine($"Email: {learner.Email}  |  Phone: {learner.Phone}").AppendLine().AppendLine($"Selected: {session.Selected.Programme} ({session.Selected.Faculty})").AppendLine($"Outcome: {session.Selected.OutcomeText}").AppendLine($"Overall recommendation score: {session.Selected.OverallScore:0.##}%").AppendLine($"Subject coverage {session.Selected.SubjectScore:0.##}% • mark match {session.Selected.MarkScore:0.##}% • interest {session.Selected.InterestScore:0.##}% • faculty preference {session.Selected.PreferenceScore:0.##}%").AppendLine().AppendLine("SUBJECTS AND MARKS");
        foreach (var mark in session.Input.Marks.OrderBy(m => m.SubjectName))
            b.AppendLine($"  {mark.SubjectName}: {mark.Mark:0.##}%");
        if (session.Input.Marks.Count > 0)
            b.AppendLine($"  Average (all captured subjects): {session.Input.Marks.Average(m => m.Mark):0.##}%");
        b.AppendLine($"Subject list confirmed complete: {(session.Input.SubjectsComplete ? "Yes" : "No")}").AppendLine().AppendLine("INTEREST CATEGORY RESULTS (0–100%)");
        foreach (var category in session.InterestScores.OrderByDescending(c => c.Value))
            b.AppendLine($"  {category.Key}: {category.Value:0.##}%");
        b.AppendLine().AppendLine("EXPLANATION AND IMPROVEMENT AREAS").AppendLine(session.Selected.Explanation).AppendLine().AppendLine("ALTERNATIVE PROGRAMMES (eligible or near-match only)");
        foreach (var r in session.Alternatives)
            b.AppendLine($"  {r.Programme} — {r.OutcomeText} — {r.OverallScore:0.##}% (interest {r.InterestScore:0.##}%)").AppendLine(r.Explanation).AppendLine();
        if (session.Alternatives.Count == 0)
            b.AppendLine("No suitable alternatives can be confirmed from the current data. Complete missing information or discuss other pathways with an advisor.");
        b.AppendLine().AppendLine("FUNDING INFORMATION");
        if (!session.Input.FundingRequested)
            b.AppendLine("Funding information was not requested in this session.");
        else
        {
            foreach (var f in funding)
                b.AppendLine($"  {f.Name} | {f.Type} | Closes {f.ClosingDate:yyyy-MM-dd}").AppendLine($"  Eligibility: {f.Eligibility}").AppendLine($"  Documents: {f.Documents}").AppendLine($"  Contact: {f.Contact}").AppendLine($"  {(f.Sample ? "SAMPLE / VERIFY WITH PROVIDER" : "Verify current details with provider")}").AppendLine();
            if (funding.Count == 0)
                b.AppendLine("No currently open, active funding records are linked to this programme or faculty. This does not establish funding ineligibility.");
        }

        b.AppendLine().AppendLine("NEXT STEPS").AppendLine("Review the gaps above; discuss alternatives with a career advisor. Verify official entry criteria before applying.").AppendLine().AppendLine(RecommendationEngine.Disclaimer).AppendLine("Privacy: this report contains learner information; share only with intended recipients.");
        return new ReportDocument
        {
            Title = "Learner Advisory Summary",
            GeneratedUtc = now,
            SessionId = session.Id,
            Content = b.ToString()
        };
    }
}

public sealed class ReportService
{
    private readonly IFuturePathStore store;
    private readonly AccessSession access;
    private readonly IClock clock;
    public ReportService(IFuturePathStore store, AccessSession access, IClock clock)
    {
        this.store = store;
        this.access = access;
        this.clock = clock;
    }

    public ReportDocument Summary(int sessionId)
    {
        access.RequireSession(sessionId);
        var session = store.GetSession(sessionId) ?? throw new ValidationException("Session not found.");
        return new ReportComposer().Summary(session, session.Input.FundingRequested ? store.GetFunding(session.Selected.DegreeId, clock.Today) : [], clock.UtcNow);
    }

    public ReportDocument Activity(DateOnly from, DateOnly to, bool exceptionsOnly, string search = "")
    {
        access.RequireStaff();
        if (from > to)
            throw new ValidationException("The start date must be on or before the end date.");
        var sessions = store.FindSessions(from, to, search.Trim());
        var rows = exceptionsOnly ? sessions.Where(s => s.Outcome is "Not Currently Eligible" or "More Information Needed").ToList() : sessions;
        var title = exceptionsOnly ? "Learners Not Currently Eligible — Exception Report" : "Degree Interest and Eligibility Report";
        var b = new StringBuilder().AppendLine("FUTURE PATH  |  " + title.ToUpperInvariant()).AppendLine($"Generated: {clock.UtcNow:u}").AppendLine($"Session dates: {from:yyyy-MM-dd} to {to:yyyy-MM-dd} (UTC, inclusive). Search: {(search.Length == 0 ? "All learners/programmes" : search)}").AppendLine($"Sessions: {rows.Count}  |  Distinct learners: {rows.Select(s => s.LearnerId).Distinct().Count()}").AppendLine();
        if (!exceptionsOnly)
        {
            b.AppendLine("OUTCOMES");
            foreach (var label in new[]
            {
                "Strong Match",
                "Possible Match",
                "Not Currently Eligible",
                "More Information Needed"
            }

            )
                b.AppendLine($"  {label}: {rows.Count(s => s.Outcome == label)}");
            b.AppendLine().AppendLine("MOST SELECTED FACULTIES");
            foreach (var g in rows.GroupBy(s => s.Faculty).OrderByDescending(g => g.Count()).ThenBy(g => g.Key))
                b.AppendLine($"  {g.Key}: {g.Count()} sessions");
            b.AppendLine().AppendLine("SELECTED DEGREES / OUTCOMES");
            foreach (var g in rows.GroupBy(s => s.Programme).OrderByDescending(g => g.Count()).ThenBy(g => g.Key))
                b.AppendLine($"  {g.Key}: {g.Count()} sessions | strong {g.Count(s => s.Outcome == "Strong Match")} | possible {g.Count(s => s.Outcome == "Possible Match")} | not eligible {g.Count(s => s.Outcome == "Not Currently Eligible")} | incomplete {g.Count(s => s.Outcome == "More Information Needed")}");
            b.AppendLine().AppendLine(ReportComposer.GapTotals(rows));
            b.AppendLine().AppendLine($"Funding requested: {rows.Count(s => s.FundingRequested)} of {rows.Count} sessions");
        }

        b.AppendLine().AppendLine(exceptionsOnly ? "LEARNERS REQUIRING FOLLOW-UP" : "SESSION DETAILS");
        foreach (var s in rows.OrderBy(s => s.Learner).ThenBy(s => s.CreatedUtc))
            b.AppendLine($"#{s.Id} | {s.Learner} | Grade {s.Grade} | {s.CreatedUtc:yyyy-MM-dd} | {s.Programme}").AppendLine($"  {s.Outcome} — {s.Score:0.##}%").AppendLine(s.Explanation).AppendLine();
        if (rows.Count == 0)
            b.AppendLine("No sessions match these filters.");
        b.AppendLine("Counts describe advisory sessions, not admission decisions. A learner can have multiple sessions.").AppendLine(RecommendationEngine.Disclaimer).AppendLine("Staff use only. Protect learner privacy.");
        return new ReportDocument
        {
            Title = title,
            GeneratedUtc = clock.UtcNow,
            Content = b.ToString()
        };
    }

    public void Export(ReportDocument report, string path)
    {
        if (report.SessionId is int id)
            access.RequireSession(id);
        else
            access.RequireStaff();
        if (string.IsNullOrWhiteSpace(path))
            throw new ValidationException("Choose an export file.");
        var content = Path.GetExtension(path).Equals(".html", StringComparison.OrdinalIgnoreCase) ? report.ToHtml() : report.Content;
        // Write to the destination volume first so a failed write does not truncate an existing report.
        var temp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            File.WriteAllText(temp, content, new UTF8Encoding(false));
            File.Move(temp, path, true);
        }
        finally
        {
            if (File.Exists(temp))
                File.Delete(temp);
        }

        if (report.SessionId is int sessionId)
            store.RecordExport(sessionId, path, clock.UtcNow);
        store.Audit(access.UserId, "Report exported", report.Title, clock.UtcNow);
    }
}
