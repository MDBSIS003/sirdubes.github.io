using FuturePathStudentAdvisingSystem.Models;
using FuturePathStudentAdvisingSystem.BusinessLogic;
using FuturePathStudentAdvisingSystem.Security;

namespace FuturePathStudentAdvisingSystem.DataAccess;

public class LearnerProfileInfo : LearnerProfile { }
public class UserAccountDAL
{
    private static readonly string DummyHash = PasswordHasher.Hash("Unused-login-timing-value");
    public bool TryLogin(string username, string password, string role, out int userID, out string displayName)
    {
        userID = 0; displayName = "";
        AppSession.End();
        if (!new[] { Roles.Learner, Roles.Advisor, Roles.Admin }.Contains(role) || username.Length > 50 || password.Length > 128) return false;
        var t = SqlStore.Table("SELECT UserID,FirstName,LastName,[Password] FROM dbo.UserAccount WHERE Username=@u AND UserRole=@r AND IsActive=1", ("@u", username.Trim()), ("@r", role));
        string hash = t.Rows.Count == 1 ? (string)t.Rows[0]["Password"] : DummyHash;
        if (!PasswordHasher.Verify(password, hash) || t.Rows.Count != 1) return false;
        userID = (int)t.Rows[0]["UserID"]; displayName = $"{t.Rows[0]["FirstName"]} {t.Rows[0]["LastName"]}";
        return true;
    }
    public bool EmailExists(string email) => SqlStore.Scalar<int>("SELECT COUNT(*) FROM dbo.UserAccount WHERE Username=@u", ("@u", email.Trim())) > 0;
    public int CreateStudentAccount(string firstName, string lastName, string email, string schoolName, string province, string password)
    {
        var p = new LearnerProfile { FirstName = firstName, LastName = lastName, Email = email, SchoolName = schoolName, Province = province };
        Validation.Profile(p, false); PasswordHasher.Validate(password);
        return SqlStore.Scalar<int>("""
            INSERT dbo.UserAccount(Username,[Password],UserRole,FirstName,LastName,SchoolName,Province)
            OUTPUT INSERTED.UserID VALUES(@u,@p,'Learner',@f,@l,@s,@v)
            """, ("@u", p.Email), ("@p", PasswordHasher.Hash(password)), ("@f", p.FirstName), ("@l", p.LastName), ("@s", p.SchoolName), ("@v", p.Province));
    }
    public LearnerProfileInfo? GetLearnerProfile(int userID)
    {
        SqlStore.ReadLearner(userID);
        var t = SqlStore.Table("SELECT * FROM dbo.UserAccount WHERE UserID=@id AND UserRole='Learner'", ("@id", userID));
        if (t.Rows.Count == 0) return null; var r = t.Rows[0];
        return new()
        {
            UserID = userID,
            FirstName = (string)r["FirstName"],
            LastName = (string)r["LastName"],
            Email = (string)r["Username"],
            SchoolName = Convert.ToString(r["SchoolName"])!,
            Province = Convert.ToString(r["Province"])!,
            Phone = Convert.ToString(r["ContactNumber"])!,
            GradeLevelID = r.IsNull("GradeLevelID") ? null : (int)r["GradeLevelID"]
        };
    }
    public void SaveProfile(LearnerProfile p)
    {
        SqlStore.Learner(p.UserID); Validation.Profile(p);
        if (SqlStore.Scalar<int>("SELECT COUNT(*) FROM dbo.GradeLevel WHERE GradeLevelID=@g AND GradeName IN ('Grade 11','Grade 12')", ("@g", p.GradeLevelID)) != 1)
            throw new ArgumentException("Select Grade 11 or Grade 12.");
        if (SqlStore.Execute("""
            UPDATE dbo.UserAccount SET FirstName=@f,LastName=@l,SchoolName=@s,Province=@v,ContactNumber=@p,GradeLevelID=@g
            WHERE UserID=@id AND UserRole='Learner'
            """, ("@f", p.FirstName), ("@l", p.LastName), ("@s", p.SchoolName), ("@v", p.Province), ("@p", p.Phone), ("@g", p.GradeLevelID), ("@id", p.UserID)) != 1)
            throw new InvalidOperationException("Learner record was not found.");
    }
    public void ChangeOwnPassword(string current, string replacement)
    {
        SqlStore.Actor(Roles.Learner, Roles.Advisor, Roles.Admin); PasswordHasher.Validate(replacement);
        var hash = SqlStore.Scalar<string>("SELECT [Password] FROM dbo.UserAccount WHERE UserID=@id", ("@id", AppSession.UserID));
        if (!PasswordHasher.Verify(current, hash)) throw new ArgumentException("Current password is incorrect.");
        SqlStore.Execute("UPDATE dbo.UserAccount SET [Password]=@p WHERE UserID=@id", ("@p", PasswordHasher.Hash(replacement)), ("@id", AppSession.UserID));
    }
}
