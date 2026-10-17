using System.Data;
using FuturePathStudentAdvisingSystem.Security;

namespace FuturePathStudentAdvisingSystem.DataAccess;

public class ReportDAL
{
    public DataTable EligibilityActivity()
    {
        SqlStore.Actor(Roles.Advisor, Roles.Admin);
        return SqlStore.Table("""
            SELECT f.FacultyName,d.ProgrammeName,r.Outcome,COUNT(*) AS Assessments,
            SUM(CASE WHEN a.FundingRequested=1 THEN 1 ELSE 0 END) AS FundingRequests
            FROM dbo.AdvisorySession a JOIN dbo.UserAccount u ON a.UserID=u.UserID
            JOIN dbo.RecommendationResult r ON r.SessionID=a.SessionID AND r.IsAlternative=0
            JOIN dbo.DegreeProgramme d ON d.DegreeID=a.DegreeID JOIN dbo.Faculty f ON f.FacultyID=a.FacultyID
            WHERE (@role='System Administrator' OR u.AdvisorUserID=@actor)
            GROUP BY f.FacultyName,d.ProgrammeName,r.Outcome ORDER BY Assessments DESC
            """, ("@role", AppSession.Role), ("@actor", AppSession.UserID));
    }
    public DataTable Exceptions()
    {
        SqlStore.Actor(Roles.Advisor, Roles.Admin);
        return SqlStore.Table("""
            SELECT u.UserID,CONCAT(u.FirstName,' ',u.LastName) AS Learner,a.SessionID,a.SessionDate,d.ProgrammeName,
            r.Outcome,r.GapExplanation FROM dbo.AdvisorySession a JOIN dbo.UserAccount u ON u.UserID=a.UserID
            JOIN dbo.DegreeProgramme d ON d.DegreeID=a.DegreeID
            JOIN dbo.RecommendationResult r ON r.SessionID=a.SessionID AND r.IsAlternative=0
            WHERE (@role='System Administrator' OR u.AdvisorUserID=@actor)
              AND r.Outcome IN ('Not Currently Eligible','More Information Needed') ORDER BY a.SessionDate DESC
            """, ("@role", AppSession.Role), ("@actor", AppSession.UserID));
    }
}
