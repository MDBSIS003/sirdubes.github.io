using System.Data;
using Microsoft.Data.SqlClient;
using FuturePathStudentAdvisingSystem.Security;

namespace FuturePathStudentAdvisingSystem.DataAccess;

internal static class SqlStore
{
    internal static SqlCommand Command(SqlConnection c, SqlTransaction? tx, string sql, params (string, object?)[] args)
    {
        var cmd = new SqlCommand(sql, c, tx);
        foreach (var (name, value) in args) cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
        return cmd;
    }
    internal static DataTable Table(string sql, params (string, object?)[] args)
    {
        using var c = DatabaseConnection.CreateConnection(); c.Open();
        using var cmd = Command(c, null, sql, args); using var reader = cmd.ExecuteReader();
        var t = new DataTable(); t.Load(reader); return t;
    }
    internal static int Execute(string sql, params (string, object?)[] args)
    {
        using var c = DatabaseConnection.CreateConnection(); c.Open();
        using var cmd = Command(c, null, sql, args); return cmd.ExecuteNonQuery();
    }
    internal static T Scalar<T>(string sql, params (string, object?)[] args)
    {
        using var c = DatabaseConnection.CreateConnection(); c.Open();
        using var cmd = Command(c, null, sql, args); return (T)Convert.ChangeType(cmd.ExecuteScalar()!, typeof(T));
    }
    // Recheck the persisted active role; stale/disabled sessions cannot continue privileged operations.
    internal static void Actor(params string[] roles)
    {
        AccessPolicy.RequireRole(roles);
        int count = Scalar<int>("SELECT COUNT(*) FROM dbo.UserAccount WHERE UserID=@id AND UserRole=@role AND IsActive=1",
            ("@id", AppSession.UserID), ("@role", AppSession.Role));
        if (count != 1) { AppSession.End(); throw new UnauthorizedAccessException("Your account changed. Please log in again."); }
    }
    internal static void Learner(int userID) { AccessPolicy.RequireLearner(userID); Actor(Roles.Learner); }
    internal static void ReadLearner(int userID)
    {
        if (AppSession.Role == Roles.Learner) { Learner(userID); return; }
        Actor(Roles.Advisor, Roles.Admin);
        if (AppSession.Role == Roles.Advisor && Scalar<int>("SELECT COUNT(*) FROM dbo.UserAccount WHERE UserID=@id AND UserRole='Learner' AND AdvisorUserID=@advisor",
            ("@id", userID), ("@advisor", AppSession.UserID)) != 1)
            throw new UnauthorizedAccessException("This learner has not been assigned to you.");
    }
    internal static int SessionOwner(int sessionID) => Scalar<int>("SELECT UserID FROM dbo.AdvisorySession WHERE SessionID=@s", ("@s", sessionID));
    internal static void ReadSession(int sessionID) => ReadLearner(SessionOwner(sessionID));
    internal static void WriteSession(int sessionID)
    {
        Learner(SessionOwner(sessionID));
        if (Scalar<int>("SELECT COUNT(*) FROM dbo.AdvisorySession WHERE SessionID=@s AND IsFinalized=0", ("@s", sessionID)) != 1)
            throw new InvalidOperationException("This is a completed session. Start a new session to change your results.");
    }
}
