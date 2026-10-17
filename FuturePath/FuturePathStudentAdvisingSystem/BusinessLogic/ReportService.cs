using System.Data;
using System.Text;
using FuturePathStudentAdvisingSystem.DataAccess;
using FuturePathStudentAdvisingSystem.Models;
using FuturePathStudentAdvisingSystem.Security;

namespace FuturePathStudentAdvisingSystem.BusinessLogic;

public class ReportService
{
    public const string Disclaimer = "Sample local guidance only. Not an official admission decision, acceptance guarantee, or verified funding offer.";
    public string Summary(AdvisingRecord session)
    {
        LearnerProfile? profile = null;
        if (session.SessionID != 0)
        { session = new AdvisingDAL().Load(session.SessionID); profile = new UserAccountDAL().GetLearnerProfile(session.UserID); }
        else if (AppSession.Role != Roles.Guest) throw new UnauthorizedAccessException("Anonymous report access denied.");
        var b = new StringBuilder("FUTURE PATH — LEARNER ADVISORY SUMMARY\n");
        b.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss zzz}\nSession: {session.SessionID}; assessed {session.SessionDate:yyyy-MM-dd HH:mm}");
        b.AppendLine(profile == null ? "Anonymous guest (not stored)" : $"Learner: {profile.FirstName} {profile.LastName}\nSchool: {profile.SchoolName}\nProvince: {profile.Province}\nContact: {profile.Email} {profile.Phone}");
        var grades = new GradeLevelDAL().GetGradeLevels();
        b.AppendLine("Grade: " + grades.AsEnumerable().FirstOrDefault(r => (int)r["GradeLevelID"] == session.GradeLevelID)?["GradeName"]);
        var selected = new DegreeProgrammeDAL().GetProgramme(session.DegreeID);
        b.AppendLine($"Selected: {selected?.ProgrammeName ?? "See stored primary recommendation"}; faculty: {selected?.FacultyName}");
        b.AppendLine("\nSUBJECTS AND MARKS"); foreach (var m in session.Marks) b.AppendLine($"{m.SubjectName}: {m.Mark:0.##}%");
        b.AppendLine("\nINTEREST RESPONSES (1 strongly disagree to 5 strongly agree)");
        var questions = new AdvisingDAL().Questions();
        foreach (var response in session.Responses) b.AppendLine($"{questions.FirstOrDefault(q => q.QuestionID == response.QuestionID)?.QuestionText}: {response.Score}");
        b.AppendLine("\nCATEGORY ALIGNMENT (0–100)");
        var categoryNames = new AdvisingDAL().Categories().AsEnumerable().ToDictionary(r => (int)r["CategoryID"], r => (string)r["CategoryName"]);
        foreach (var g in questions.Where(q => session.Responses.Any(r => r.QuestionID == q.QuestionID)).GroupBy(q => q.CategoryID))
            b.AppendLine($"{categoryNames.GetValueOrDefault(g.Key, "Category " + g.Key)}: {g.Average(q => (session.Responses.First(r => r.QuestionID == q.QuestionID).Score - 1) * 25):0.##}%");
        b.AppendLine("\nSELECTED DEGREE AND ALTERNATIVES");
        if (session.SessionID != 0 && session.IsFinalized)
            b.AppendLine(TableText(new AdvisingDAL().StoredRecommendations(session.SessionID)));
        else foreach (var r in AdvisingWorkflow.Calculate(session).OrderBy(r => r.IsAlternative))
            b.AppendLine($"{(r.IsAlternative ? "Alternative" : "Selected")}: {r.ProgrammeName} — {r.Outcome}, {r.OverallScore:0.##}\n{r.Explanation}");
        if (session.FundingRequested)
        {
            b.AppendLine("\nRELEVANT FUNDING (current listings; verify before applying)");
            foreach (var f in new AdvisingDAL().Funding(degreeID: session.DegreeID))
                b.AppendLine($"{f.FundingName} [{(f.IsSample ? "SAMPLE" : "REQUIRES VERIFICATION")}] {f.FundingType}\n{f.EligibilityDescription}\nDocuments: {f.RequiredDocuments}\nClosing: {f.ClosingDate:yyyy-MM-dd}\nContact: {f.ContactInfo}");
        }
        b.AppendLine("\n" + Disclaimer); return b.ToString();
    }
    public static string TableText(DataTable t)
    {
        var b = new StringBuilder(); b.AppendLine(string.Join(" | ", t.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));
        foreach (DataRow r in t.Rows) b.AppendLine(string.Join(" | ", r.ItemArray.Select(v => Convert.ToString(v)?.Replace("\r", " ").Replace("\n", " "))));
        if (t.Rows.Count == 0) b.AppendLine("No records match this report.");
        return b.ToString();
    }
}
