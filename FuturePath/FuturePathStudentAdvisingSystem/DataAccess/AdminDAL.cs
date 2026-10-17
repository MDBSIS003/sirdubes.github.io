using FuturePathStudentAdvisingSystem.Models;
using System.Data;
using FuturePathStudentAdvisingSystem.BusinessLogic;
using FuturePathStudentAdvisingSystem.Security;

namespace FuturePathStudentAdvisingSystem.DataAccess;

/// <summary>Explicit allowlist: UI labels or user input can never become SQL identifiers.</summary>
public class AdminDAL
{
    public static readonly ManagedTable[] Definitions = [
        new("Faculty","FacultyID",new ManagedField("FacultyName",typeof(string),true,100)),
        new("Subject","SubjectID",new ManagedField("SubjectName",typeof(string),true,150)),
        new("DegreeProgramme","DegreeID",new ManagedField("FacultyID",typeof(int),true,0,"Faculty"),new ManagedField("ProgrammeName",typeof(string),true,150),new ManagedField("Description",typeof(string),false,4000),new ManagedField("IsActive",typeof(bool))),
        new("ProgrammeRequirement","RequirementID",new ManagedField("DegreeID",typeof(int),true,0,"DegreeProgramme"),new ManagedField("SubjectID",typeof(int),true,0,"Subject"),new ManagedField("MinimumMark",typeof(decimal)),new ManagedField("IsMandatory",typeof(bool)),new ManagedField("AlternativeGroup",typeof(string),false,50),new ManagedField("AdvisoryNote",typeof(string),false,4000)),
        new("InterestCategory","CategoryID",new ManagedField("CategoryName",typeof(string),true,100)),
        new("InterestQuestion","QuestionID",new ManagedField("CategoryID",typeof(int),true,0,"InterestCategory"),new ManagedField("QuestionText",typeof(string),true,2000)),
        new("DegreeInterestMapping","MappingID",new ManagedField("DegreeID",typeof(int),true,0,"DegreeProgramme"),new ManagedField("CategoryID",typeof(int),true,0,"InterestCategory"),new ManagedField("Weight",typeof(decimal))),
        new("FundingOption","FundingID",new ManagedField("FacultyID",typeof(int),true,0,"Faculty"),new ManagedField("FundingName",typeof(string),true,250),new ManagedField("FundingType",typeof(string),true,100),new ManagedField("EligibilityDescription",typeof(string),false,4000),new ManagedField("RequiredDocuments",typeof(string),false,4000),new ManagedField("ClosingDate",typeof(DateTime),false),new ManagedField("ContactInfo",typeof(string),false,250),new ManagedField("InActive",typeof(bool)),new ManagedField("IsSample",typeof(bool))),
        new("FundingProgramme","FundingProgrammeID",new ManagedField("FundingID",typeof(int),true,0,"FundingOption"),new ManagedField("DegreeID",typeof(int),true,0,"DegreeProgramme")),
        new("UserAccount","UserID",new ManagedField("Username",typeof(string),true,50),new ManagedField("UserRole",typeof(string),true,20),new ManagedField("FirstName",typeof(string),true,50),new ManagedField("LastName",typeof(string),true,50),new ManagedField("SchoolName",typeof(string),false,100),new ManagedField("Province",typeof(string),false,50),new ManagedField("IsActive",typeof(bool)),new ManagedField("AdvisorUserID",typeof(int),false,0,"Advisor"))
    ];
    public static ManagedTable Definition(string table) => Definitions.SingleOrDefault(x => x.Name == table) ?? throw new ArgumentException("Unknown management table.");
    public DataTable List(string table)
    {
        SqlStore.Actor(Roles.Admin); var d = Definition(table);
        return SqlStore.Table($"SELECT [{d.Key}],{string.Join(',', d.Fields.Select(f => $"[{f.Name}]"))} FROM dbo.[{d.Name}] ORDER BY [{d.Key}]");
    }
    public DataTable Lookup(string table)
    {
        SqlStore.Actor(Roles.Admin);
        if (table == "Advisor") return SqlStore.Table("SELECT UserID AS ID,CONCAT(FirstName,' ',LastName,' (#',UserID,')') AS Label FROM dbo.UserAccount WHERE UserRole='Career Advisor' AND IsActive=1");
        var d = Definition(table); string label = d.Fields.First(f => f.Type == typeof(string)).Name;
        return SqlStore.Table($"SELECT [{d.Key}] AS ID,CONCAT([{label}],' (#',[{d.Key}],')') AS Label FROM dbo.[{d.Name}]");
    }
    public void Save(string table, int? id, IReadOnlyDictionary<string, object?> values, string? initialPassword = null)
    {
        SqlStore.Actor(Roles.Admin); var d = Definition(table);
        var args = ReferenceDataValidation.Validate(d, values);
        if (table == "UserAccount")
        {
            string role = Convert.ToString(values["UserRole"])!;
            if (!new[] { Roles.Learner, Roles.Advisor, Roles.Admin }.Contains(role)) throw new ArgumentException("Select a valid role.");
            if (id == AppSession.UserID && (role != Roles.Admin || !Convert.ToBoolean(values["IsActive"])))
                throw new ArgumentException("You cannot disable or demote your current administrator account.");
            if (values["AdvisorUserID"] is int advisor && SqlStore.Scalar<int>("SELECT COUNT(*) FROM dbo.UserAccount WHERE UserID=@a AND UserRole='Career Advisor' AND IsActive=1", ("@a", advisor)) != 1)
                throw new ArgumentException("Select an active career advisor.");
            if (values["AdvisorUserID"] is int && role != Roles.Learner) throw new ArgumentException("Only learners can be assigned to an advisor.");
            if (id == null) { PasswordHasher.Validate(initialPassword ?? ""); args.Add(("@Password", PasswordHasher.Hash(initialPassword!))); }
            else if (role != Roles.Learner && SqlStore.Scalar<int>("SELECT COUNT(*) FROM dbo.AdvisorySession WHERE UserID=@id", ("@id", id)) > 0)
                throw new ArgumentException("A learner with advising history must retain the learner role. Deactivate the account instead.");
        }
        if (id is int key)
        {
            args.Add(("@id", key));
            if (SqlStore.Execute($"UPDATE dbo.[{d.Name}] SET {string.Join(',', d.Fields.Select(f => $"[{f.Name}]=@{f.Name}"))} WHERE [{d.Key}]=@id", args.ToArray()) != 1)
                throw new InvalidOperationException("Record no longer exists.");
        }
        else
        {
            var cols = d.Fields.Select(f => f.Name).ToList(); if (table == "UserAccount") cols.Add("Password");
            SqlStore.Execute($"INSERT dbo.[{d.Name}]({string.Join(',', cols.Select(x => $"[{x}]"))}) VALUES({string.Join(',', cols.Select(x => "@" + x))})", args.ToArray());
        }
    }
    public void Delete(string table, int id)
    {
        SqlStore.Actor(Roles.Admin); var d = Definition(table);
        if (table == "UserAccount")
        {
            if (id == AppSession.UserID) throw new ArgumentException("You cannot deactivate your current account.");
            SqlStore.Execute("UPDATE dbo.UserAccount SET IsActive=0 WHERE UserID=@id", ("@id", id));
        }
        else if (table == "DegreeProgramme") SqlStore.Execute("UPDATE dbo.DegreeProgramme SET IsActive=0 WHERE DegreeID=@id", ("@id", id));
        else if (table == "FundingOption") SqlStore.Execute("UPDATE dbo.FundingOption SET InActive=1 WHERE FundingID=@id", ("@id", id));
        else SqlStore.Execute($"DELETE dbo.[{d.Name}] WHERE [{d.Key}]=@id", ("@id", id));
    }
    public void ResetPassword(int userID, string replacement)
    {
        SqlStore.Actor(Roles.Admin); PasswordHasher.Validate(replacement);
        if (SqlStore.Execute("UPDATE dbo.UserAccount SET [Password]=@p WHERE UserID=@id", ("@p", PasswordHasher.Hash(replacement)), ("@id", userID)) != 1)
            throw new ArgumentException("Account not found.");
    }
}
