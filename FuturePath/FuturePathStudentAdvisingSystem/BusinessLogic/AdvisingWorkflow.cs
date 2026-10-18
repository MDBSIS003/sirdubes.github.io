using System.Data;
using FuturePathStudentAdvisingSystem.DataAccess;
using FuturePathStudentAdvisingSystem.Models;
using FuturePathStudentAdvisingSystem.Security;

namespace FuturePathStudentAdvisingSystem.BusinessLogic;

/// <summary>Single-user working session. Anonymous results are memory-only and cleared on logout.</summary>
public static class AdvisingWorkflow
{
    public static AdvisingRecord? Current { get; private set; }
    public static void Clear() => Current = null;
    public static void Load(int sessionID)
    {
        AccessPolicy.RequireRole(Roles.Learner);
        Current = new AdvisingDAL().Load(sessionID);
    }
    public static void Begin(int gradeID, int degreeID)
    {
        AccessPolicy.RequirePage("Profile");
        var d = new DegreeProgrammeDAL().GetActiveDegreeProgrammes().SingleOrDefault(d => d.DegreeID == degreeID)
            ?? throw new ArgumentException("Select an active degree.");
        var grades = new GradeLevelDAL().GetGradeLevels();
        if (!grades.AsEnumerable().Any(r => (int)r["GradeLevelID"] == gradeID && new[] { "Grade 11", "Grade 12" }.Contains((string)r["GradeName"])))
            throw new ArgumentException("Select Grade 11 or Grade 12.");
        int id = AppSession.UserID ?? 0;
        int sessionID = id == 0 ? 0 : new AdvisingDAL().CreateSession(id, gradeID, degreeID);
        Current = new() { SessionID = sessionID, UserID = id, GradeLevelID = gradeID, DegreeID = degreeID, FacultyID = d.FacultyID, SessionDate = DateTime.Now };
    }
    public static AdvisingRecord RequireCurrent()
    {
        AccessPolicy.RequirePage("Profile");
        return Current ?? throw new InvalidOperationException("Start or load an advising session on the profile screen first.");
    }
    public static void SaveMarks(List<SubjectMark> marks, bool complete)
    {
        var s = RequireCurrent(); if (s.IsFinalized) throw new InvalidOperationException("Start a new session before changing completed results.");
        Validation.Marks(marks);
        if (s.SessionID != 0) new AdvisingDAL().SaveMarks(s.SessionID, marks, complete);
        s.Marks = marks.ToList(); s.SubjectsComplete = complete;
    }
    public static void SaveResponses(List<InterestResponse> responses)
    {
        var s = RequireCurrent(); if (s.IsFinalized) throw new InvalidOperationException("Start a new session before changing completed results.");
        Validation.Responses(responses, new AdvisingDAL().Questions());
        if (s.SessionID != 0) new AdvisingDAL().SaveResponses(s.SessionID, responses);
        s.Responses = responses.ToList();
    }
    public static List<Recommendation> Calculate(AdvisingRecord s) => new RecommendationService().Rank(s,
        new DegreeProgrammeDAL().GetActiveDegreeProgrammes(), new AdvisingDAL().Requirements(), new AdvisingDAL().Questions(), new AdvisingDAL().Mappings());
    public static List<Recommendation> Complete(bool funding)
    {
        var s = RequireCurrent();
        if (s.IsFinalized) throw new InvalidOperationException("This session is already completed. View stored results or start a new session.");
        var results = Calculate(s);
        if (!results.Any(x => x.DegreeID == s.DegreeID)) throw new InvalidOperationException("Selected degree was deactivated. Start a new session with an active degree.");
        if (s.SessionID != 0) new AdvisingDAL().SaveRecommendations(s.SessionID, results, funding);
        s.FundingRequested = funding; s.IsFinalized = true;
        return results;
    }
}
