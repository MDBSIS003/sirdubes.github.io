using System.Data;
using FuturePathStudentAdvisingSystem.Models;
using FuturePathStudentAdvisingSystem.Security;
using FuturePathStudentAdvisingSystem.BusinessLogic;

namespace FuturePathStudentAdvisingSystem.DataAccess;

public class AdvisingDAL
{
    public List<InterestQuestion> Questions() => SqlStore.Table("SELECT QuestionID,CategoryID,QuestionText FROM dbo.InterestQuestion ORDER BY QuestionID")
        .AsEnumerable().Select(r => new InterestQuestion((int)r[0], (int)r[1], (string)r[2])).ToList();
    public List<ProgrammeRequirement> Requirements() => SqlStore.Table("""
        SELECT p.RequirementID,p.DegreeID,p.SubjectID,s.SubjectName,p.MinimumMark,p.IsMandatory,p.AlternativeGroup,p.AdvisoryNote
        FROM dbo.ProgrammeRequirement p JOIN dbo.Subject s ON s.SubjectID=p.SubjectID
        """).AsEnumerable().Select(r => new ProgrammeRequirement((int)r[0], (int)r[1], (int)r[2], (string)r[3], (decimal)r[4], (bool)r[5],
            r.IsNull(6) ? null : (string)r[6], Convert.ToString(r[7])!)).ToList();
    public List<InterestMapping> Mappings() => SqlStore.Table("SELECT DegreeID,CategoryID,Weight FROM dbo.DegreeInterestMapping")
        .AsEnumerable().Select(r => new InterestMapping((int)r[0], (int)r[1], (decimal)r[2])).ToList();
    public DataTable Categories() => SqlStore.Table("SELECT CategoryID,CategoryName FROM dbo.InterestCategory ORDER BY CategoryName");
    public DataTable Faculties() => SqlStore.Table("SELECT FacultyID,FacultyName FROM dbo.Faculty ORDER BY FacultyName");
    public DataTable Sessions(int userID)
    {
        SqlStore.ReadLearner(userID);
        return SqlStore.Table("""
            SELECT a.SessionID,a.SessionDate,d.ProgrammeName,a.IsFinalized FROM dbo.AdvisorySession a
            JOIN dbo.DegreeProgramme d ON d.DegreeID=a.DegreeID WHERE a.UserID=@id ORDER BY a.SessionID DESC
            """, ("@id", userID));
    }
    public int CreateSession(int userID, int gradeID, int degreeID)
    {
        SqlStore.Learner(userID);
        return SqlStore.Scalar<int>("""
            INSERT dbo.AdvisorySession(UserID,GradeLevelID,FacultyID,DegreeID)
            OUTPUT INSERTED.SessionID SELECT @u,g.GradeLevelID,d.FacultyID,d.DegreeID
            FROM dbo.DegreeProgramme d CROSS JOIN dbo.GradeLevel g
            WHERE d.DegreeID=@d AND d.IsActive=1 AND g.GradeLevelID=@g AND g.GradeName IN ('Grade 11','Grade 12')
            """, ("@u", userID), ("@d", degreeID), ("@g", gradeID));
    }
    public AdvisingRecord Load(int sessionID)
    {
        SqlStore.ReadSession(sessionID);
        var r = SqlStore.Table("SELECT * FROM dbo.AdvisorySession WHERE SessionID=@s", ("@s", sessionID)).Rows[0];
        return new()
        {
            SessionID = sessionID,
            UserID = (int)r["UserID"],
            GradeLevelID = (int)r["GradeLevelID"],
            FacultyID = (int)r["FacultyID"],
            DegreeID = (int)r["DegreeID"],
            SubjectsComplete = (bool)r["SubjectsComplete"],
            FundingRequested = (bool)r["FundingRequested"],
            IsFinalized = (bool)r["IsFinalized"],
            SessionDate = (DateTime)r["SessionDate"],
            Marks = SqlStore.Table("SELECT s.SubjectID,s.SubjectName,l.Mark FROM dbo.LearnerSubject l JOIN dbo.Subject s ON l.SubjectID=s.SubjectID WHERE l.SessionID=@s", ("@s", sessionID))
                .AsEnumerable().Select(x => new SubjectMark((int)x[0], (string)x[1], (decimal)x[2])).ToList(),
            Responses = SqlStore.Table("SELECT QuestionID,Score FROM dbo.LearnerInterestResponse WHERE SessionID=@s", ("@s", sessionID))
                .AsEnumerable().Select(x => new InterestResponse((int)x[0], (decimal)x[1])).ToList()
        };
    }
    public void SaveMarks(int sessionID, List<SubjectMark> marks, bool complete)
    {
        SqlStore.WriteSession(sessionID); Validation.Marks(marks);
        using var c = DatabaseConnection.CreateConnection(); c.Open(); using var tx = c.BeginTransaction();
        using (var cmd = SqlStore.Command(c, tx, "DELETE dbo.LearnerSubject WHERE SessionID=@s", ("@s", sessionID))) cmd.ExecuteNonQuery();
        foreach (var m in marks)
        { using var cmd = SqlStore.Command(c, tx, "INSERT dbo.LearnerSubject(SessionID,SubjectID,Mark) VALUES(@s,@id,@m)", ("@s", sessionID), ("@id", m.SubjectID), ("@m", m.Mark)); cmd.ExecuteNonQuery(); }
        using (var cmd = SqlStore.Command(c, tx, "UPDATE dbo.AdvisorySession SET SubjectsComplete=@v WHERE SessionID=@s; DELETE dbo.RecommendationResult WHERE SessionID=@s", ("@s", sessionID), ("@v", complete))) cmd.ExecuteNonQuery();
        tx.Commit();
    }
    public void SaveResponses(int sessionID, List<InterestResponse> responses)
    {
        SqlStore.WriteSession(sessionID); Validation.Responses(responses, Questions());
        using var c = DatabaseConnection.CreateConnection(); c.Open(); using var tx = c.BeginTransaction();
        using (var cmd = SqlStore.Command(c, tx, "DELETE dbo.LearnerInterestResponse WHERE SessionID=@s; DELETE dbo.RecommendationResult WHERE SessionID=@s", ("@s", sessionID))) cmd.ExecuteNonQuery();
        foreach (var r in responses)
        { using var cmd = SqlStore.Command(c, tx, "INSERT dbo.LearnerInterestResponse(SessionID,QuestionID,Score) VALUES(@s,@q,@v)", ("@s", sessionID), ("@q", r.QuestionID), ("@v", r.Score)); cmd.ExecuteNonQuery(); }
        tx.Commit();
    }
    public void SaveRecommendations(int sessionID, List<Recommendation> results, bool funding)
    {
        SqlStore.WriteSession(sessionID);
        using var c = DatabaseConnection.CreateConnection(); c.Open(); using var tx = c.BeginTransaction();
        using (var cmd = SqlStore.Command(c, tx, "DELETE dbo.RecommendationResult WHERE SessionID=@s", ("@s", sessionID))) cmd.ExecuteNonQuery();
        foreach (var r in results)
        { using var cmd = SqlStore.Command(c, tx, """
            INSERT dbo.RecommendationResult(SessionID,DegreeID,Outcome,OverallScore,GapExplanation,IsAlternative)
            VALUES(@s,@d,@o,@v,@e,@a)
            """, ("@s", sessionID), ("@d", r.DegreeID), ("@o", r.Outcome), ("@v", r.OverallScore), ("@e", r.Explanation), ("@a", r.IsAlternative)); cmd.ExecuteNonQuery(); }
        using (var cmd = SqlStore.Command(c, tx, "UPDATE dbo.AdvisorySession SET FundingRequested=@f,IsFinalized=1 WHERE SessionID=@s", ("@s", sessionID), ("@f", funding))) cmd.ExecuteNonQuery();
        tx.Commit();
    }
    public DataTable StoredRecommendations(int sessionID)
    {
        SqlStore.ReadSession(sessionID);
        return SqlStore.Table("""
            SELECT d.ProgrammeName,r.Outcome,r.OverallScore,r.GapExplanation,r.IsAlternative FROM dbo.RecommendationResult r
            JOIN dbo.DegreeProgramme d ON d.DegreeID=r.DegreeID WHERE r.SessionID=@s ORDER BY r.IsAlternative,r.OverallScore DESC
            """, ("@s", sessionID));
    }
    public List<FundingOpportunity> Funding(string search = "", string type = "", int? degreeID = null, int? facultyID = null)
    {
        return SqlStore.Table("""
            SELECT f.FundingID,f.FundingName,f.FundingType,f.EligibilityDescription,f.RequiredDocuments,f.ClosingDate,f.ContactInfo,fa.FacultyName,f.IsSample
            FROM dbo.FundingOption f JOIN dbo.Faculty fa ON fa.FacultyID=f.FacultyID
            WHERE f.InActive=0 AND (f.ClosingDate IS NULL OR f.ClosingDate>=CONVERT(date,GETDATE()))
            AND f.FundingName LIKE @search AND (@type='' OR f.FundingType=@type)
            AND (@faculty IS NULL OR f.FacultyID=@faculty)
            AND (@degree IS NULL OR EXISTS(SELECT 1 FROM dbo.FundingProgramme fp WHERE fp.FundingID=f.FundingID AND fp.DegreeID=@degree)
              OR (NOT EXISTS(SELECT 1 FROM dbo.FundingProgramme fp WHERE fp.FundingID=f.FundingID)
                  AND f.FacultyID=(SELECT FacultyID FROM dbo.DegreeProgramme WHERE DegreeID=@degree)))
            ORDER BY f.FundingName
            """, ("@search", "%" + search + "%"), ("@type", type), ("@degree", degreeID), ("@faculty", facultyID))
            .AsEnumerable().Select(r => new FundingOpportunity((int)r[0], (string)r[1], (string)r[2], Convert.ToString(r[3])!, Convert.ToString(r[4])!,
                r.IsNull(5) ? null : (DateTime)r[5], Convert.ToString(r[6])!, (string)r[7], (bool)r[8])).ToList();
    }
    public DataTable FundingTypes() => SqlStore.Table("SELECT DISTINCT FundingType FROM dbo.FundingOption WHERE InActive=0 ORDER BY FundingType");
    public DataTable SearchLearners(string text)
    {
        SqlStore.Actor(Roles.Advisor, Roles.Admin);
        return SqlStore.Table("""
            SELECT UserID,FirstName,LastName,SchoolName,Province FROM dbo.UserAccount WHERE UserRole='Learner'
            AND (@role='System Administrator' OR AdvisorUserID=@actor)
            AND (CONCAT(FirstName,' ',LastName) LIKE @q OR CONVERT(varchar(20),UserID)=@id)
            ORDER BY LastName,FirstName
            """, ("@role", AppSession.Role), ("@actor", AppSession.UserID), ("@q", "%" + text + "%"), ("@id", text));
    }
    public DataTable Notes(int sessionID)
    {
        SqlStore.ReadSession(sessionID);
        return SqlStore.Table("SELECT n.CreatedAt,u.FirstName,u.LastName,n.NoteText FROM dbo.AdvisingNote n JOIN dbo.UserAccount u ON u.UserID=n.AdvisorUserID WHERE SessionID=@s ORDER BY n.CreatedAt", ("@s", sessionID));
    }
    public void AddNote(int sessionID, string note)
    {
        SqlStore.Actor(Roles.Advisor); SqlStore.ReadSession(sessionID);
        note = Validation.Required(note, "Advising note", 2000);
        SqlStore.Execute("INSERT dbo.AdvisingNote(SessionID,AdvisorUserID,NoteText) VALUES(@s,@a,@n)", ("@s", sessionID), ("@a", AppSession.UserID), ("@n", note));
    }
    public void RecordExport(int sessionID)
    {
        SqlStore.ReadSession(sessionID);
        SqlStore.Execute("INSERT dbo.ExportReport(SessionID) VALUES(@s)", ("@s", sessionID));
    }
}
