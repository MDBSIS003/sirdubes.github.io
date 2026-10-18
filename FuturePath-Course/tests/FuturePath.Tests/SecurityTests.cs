using FuturePath.Business;

namespace FuturePath.Tests;
public sealed class SecurityTests
{
    [Fact]
    public void PasswordsUseUniqueSaltsAndVerifyWithoutPlaintextStorage()
    {
        var h = new PasswordHasher();
        var a = h.Hash("TestingPassword12");
        var b = h.Hash("TestingPassword12");
        Assert.NotEqual(a, b);
        Assert.True(h.Verify("TestingPassword12", a));
        Assert.False(h.Verify("WrongPassword12", a));
        Assert.DoesNotContain("TestingPassword", a);
        Assert.False(h.Verify("anything", "invalid"));
    }

    [LocalDbFact]
    public void GuestsCannotReadOtherLearnersOrAccessStaffServices()
    {
        using var db = new TestDatabase();
        Assert.Throws<AccessDeniedException>(() => db.Advising.Load(1));
        Assert.Throws<AccessDeniedException>(() => db.Reports.Summary(1));
        Assert.Throws<AccessDeniedException>(() => db.Advising.Search(DateOnly.MinValue, DateOnly.MaxValue));
        Assert.Throws<AccessDeniedException>(() => db.Maintenance.Rows("DegreeProgramme"));
        Assert.Throws<AccessDeniedException>(() => db.Maintenance.Save("Faculty", 0, new Dictionary<string, object?>()));
        Assert.Throws<AccessDeniedException>(() => db.Auth.SaveUser(0, "intruder", UserRole.Administrator, true, "TestingPassword12"));
    }

    [LocalDbFact]
    public void LearnerCanReadOnlySessionsCreatedInCurrentWorkspace()
    {
        using var db = new TestDatabase();
        var session = db.Advising.Assess(db.Input());
        Assert.Equal(session.Id, db.Advising.Load(session.Id).Id);
        db.Access.SignOut();
        Assert.Throws<AccessDeniedException>(() => db.Advising.Load(session.Id));
    }

    [LocalDbFact]
    public void AdvisorCanReadAndReportButCannotMaintainReferenceDataOrAccounts()
    {
        using var db = new TestDatabase();
        db.Advisor();
        Assert.Equal(UserRole.Advisor, db.Access.Role);
        Assert.Equal(10, db.Advising.Search(DateOnly.MinValue, DateOnly.MaxValue).Count);
        Assert.NotEmpty(db.Reports.Summary(1).Content);
        Assert.Throws<AccessDeniedException>(() => db.Maintenance.Rows("Faculty"));
        Assert.Throws<AccessDeniedException>(() => db.Auth.SaveUser(0, "advisor2", UserRole.Advisor, true, "TestingPassword12"));
    }

    [LocalDbFact]
    public void BadPasswordAndInjectionDoNotGrantAccess()
    {
        using var db = new TestDatabase();
        Assert.Throws<ValidationException>(() => db.Auth.Login("Adonias", "incorrect"));
        Assert.Throws<ValidationException>(() => db.Auth.Login("' OR 1=1 --", "incorrect"));
        Assert.Equal(UserRole.Learner, db.Access.Role);
    }

    [LocalDbFact]
    public void FifthFailureLocksAccountAndExpiryAllowsSuccessfulLogin()
    {
        using var db = new TestDatabase();
        for (var i = 0; i < 5; i++)
            Assert.Throws<ValidationException>(() => db.Auth.Login("SIRDUBES", "incorrect"));
        Assert.NotNull(db.Store.FindUser("SIRDUBES")!.LockedUntilUtc);
        Assert.Throws<ValidationException>(() => db.Advisor());
        db.Clock.UtcNow = db.Clock.UtcNow.AddMinutes(6);
        db.Advisor();
        Assert.Equal(UserRole.Advisor, db.Access.Role);
        Assert.Equal(0, db.Store.FindUser("SIRDUBES")!.FailedAttempts);
    }

    [LocalDbFact]
    public void AdminCanCreateDisableAndResetAccounts()
    {
        using var db = new TestDatabase();
        db.Admin();
        db.Auth.SaveUser(0, "new.advisor", UserRole.Advisor, true, "TestingPassword12");
        var user = db.Store.FindUser("new.advisor")!;
        db.Auth.SaveUser(user.Id, user.Username, user.Role, false, null);
        db.Access.SignOut();
        Assert.Throws<ValidationException>(() => db.Auth.Login("new.advisor", "TestingPassword12"));
        db.Admin();
        db.Auth.SaveUser(user.Id, user.Username, user.Role, true, "ReplacementPass12");
        db.Access.SignOut();
        db.Auth.Login("new.advisor", "ReplacementPass12");
        Assert.Equal(UserRole.Advisor, db.Access.Role);
    }

    [LocalDbFact]
    public void CurrentAdminCannotBeDemotedOrDisabled()
    {
        using var db = new TestDatabase();
        db.Admin();
        Assert.Throws<ValidationException>(() => db.Auth.SaveUser(db.Access.UserId!.Value, "Adonias", UserRole.Administrator, false, null));
        Assert.Throws<ValidationException>(() => db.Auth.SaveUser(db.Access.UserId!.Value, "Adonias", UserRole.Advisor, true, null));
    }

    [LocalDbFact]
    public void PasswordChangeVerifiesOldPasswordAndAppliesNewPassword()
    {
        using var db = new TestDatabase();
        db.Advisor();
        Assert.Throws<ValidationException>(() => db.Auth.ChangePassword("wrong", "ReplacementPass12"));
        db.Auth.ChangePassword("MDBSI003", "ReplacementPass12");
        db.Access.SignOut();
        Assert.Throws<ValidationException>(() => db.Advisor());
        db.Auth.Login("SIRDUBES", "ReplacementPass12");
        Assert.True(db.Access.IsStaff);
    }

    [LocalDbFact]
    public void UserListingsNeverExposeHashes()
    {
        using var db = new TestDatabase();
        db.Admin();
        Assert.All(db.Maintenance.Rows("UserAccount"), row => Assert.DoesNotContain("PasswordHash", row.Values.Keys));
    }
}

public sealed class SecurityAndSearchPersistenceTests
{
    [LocalDbTheory]
    [InlineData(false)]
    [InlineData(true)]
    public void SecuritySettingsSurviveReconnectAndNeverAlterRole(bool replacePassword)
    {
        using TestDatabase db = new();
        db.Advisor();
        int id = db.Access.UserId!.Value;
        string password = replacePassword ? "ReplacementPass2026!" : "MDBSI003";
        db.Auth.ChangeSecurity("qa.advisor", "MDBSI003", replacePassword ? password : "");
        var store = new FuturePath.Data.SqlServerStore(db.Store.DatabaseName);
        var user = store.FindUser("qa.advisor")!;
        Assert.Equal(id,user.Id);
        Assert.Equal(UserRole.Advisor,user.Role);
        Assert.True(new PasswordHasher().Verify(password,user.PasswordHash));
        Assert.Null(store.FindUser("SIRDUBES"));
        db.Access.SignOut();
        db.Auth.Login("qa.advisor",password);
        Assert.Throws<AccessDeniedException>(() => db.Maintenance.Rows("Faculty"));
    }
    [LocalDbFact]
    public void DuplicateUsernameRollsBackPasswordAndLeavesBothAccountsUsable()
    {
        using TestDatabase db = new(); db.Advisor();
        Assert.Throws<ValidationException>(() => db.Auth.ChangeSecurity("ADONIAS","MDBSI003","ReplacementPass2026!"));
        db.Access.SignOut(); db.Advisor(); db.Access.SignOut(); db.Admin();
        Assert.Equal(UserRole.Administrator,db.Access.Role);
    }
    [LocalDbFact]
    public void SecurityAuditFailureRollsBackAccountUpdate()
    {
        using TestDatabase db = new(); db.Advisor();
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(db.Store.ConnectionString);
        connection.Open();
        using (var trigger = connection.CreateCommand())
        {
            trigger.CommandText = "CREATE TRIGGER dbo.QA_BlockSecurityAudit ON dbo.AuditEvent AFTER INSERT AS BEGIN THROW 51001, 'Controlled audit failure', 1; END;";
            trigger.ExecuteNonQuery();
        }
        Assert.Throws<Microsoft.Data.SqlClient.SqlException>(() => db.Auth.ChangeSecurity("qa.advisor","MDBSI003","ReplacementPass2026!"));
        Assert.Equal("SIRDUBES",db.Access.DisplayName);
        Assert.Null(db.Store.FindUser("qa.advisor"));
        Assert.True(new PasswordHasher().Verify("MDBSI003",db.Store.FindUser("SIRDUBES")!.PasswordHash));
    }
    [LocalDbFact]
    public void StaffSearchFindsCompletedStudentAfterNewStoreConnection()
    {
        using TestDatabase db = new();
        var input = db.Input();
        input.Learner.Name = "QA Learner";
        input.Learner.School = "QA Secondary";
        input.Learner.Email = "qa.unique@example.com";
        input.Learner.Phone = "0712345678";
        var session = db.Advising.Assess(input);
        db.Access.SignOut();
        var reopened = new FuturePath.Data.SqlServerStore(db.Store.DatabaseName);
        var access = new AccessSession();
        new AuthenticationController(reopened,access,db.Clock).Login("SIRDUBES","MDBSI003");
        var advising = new AdvisorySessionController(reopened,access,db.Clock);
        foreach (string query in new[] { "QA Learner","QA Secondary","qa.unique@example.com","0712345678","FP-"+session.Id })
            Assert.Contains(advising.Search(DateOnly.MinValue,DateOnly.MaxValue,query), r => r.Id==session.Id);
        var saved=advising.Load(session.Id);
        Assert.Equal(input.Marks.Count,saved.Input.Marks.Count);
        Assert.Equal(input.Responses.Count,saved.Input.Responses.Count);
        Assert.Equal(session.Selected.OverallScore,saved.Selected.OverallScore);
        Assert.Equal(session.Alternatives.Count,saved.Alternatives.Count);
    }
    [LocalDbTheory]
    [InlineData("Faculty")]
    [InlineData("Subject")]
    [InlineData("InterestCategory")]
    [InlineData("DegreeProgramme")]
    [InlineData("ProgrammeRequirement")]
    [InlineData("InterestQuestion")]
    [InlineData("DegreeInterestMapping")]
    [InlineData("FundingOption")]
    [InlineData("FundingFaculty")]
    [InlineData("FundingProgramme")]
    public void EveryReferenceEditorCanSaveAndReopenItsUnchangedRecord(string key)
    {
        using TestDatabase db = new(); db.Admin();
        var row=db.Maintenance.Rows(key).First();
        var values=new Dictionary<string,object?>(row.Values);
        Assert.Equal(row.Id,db.Maintenance.Save(key,row.Id,values));
        var reloaded=db.Maintenance.Rows(key).Single(r=>r.Id==row.Id);
        foreach (string column in values.Keys) Assert.Equal(Convert.ToString(values[column]),Convert.ToString(reloaded.Values[column]));
    }
}

public sealed class MarkerAccountPersistenceTests
{
    [LocalDbTheory]
    [InlineData(UserRole.Advisor)]
    [InlineData(UserRole.Administrator)]
    public void CreatedMarkerLoginWorksAfterReconnect(UserRole role)
    {
        using TestDatabase db = new(); db.Admin();
        db.Auth.SaveUser(0,"marker.account",role,true,"MarkerPassword2026!");
        var store=new FuturePath.Data.SqlServerStore(db.Store.DatabaseName);
        var access=new AccessSession();
        new AuthenticationController(store,access,db.Clock).Login("marker.account","MarkerPassword2026!");
        Assert.Equal(role,access.Role);
        Assert.Equal(10,new AdvisorySessionController(store,access,db.Clock).Search(DateOnly.MinValue,DateOnly.MaxValue).Count);
        if (role==UserRole.Advisor)
            Assert.Throws<AccessDeniedException>(()=>new AdminMaintenanceController(store,access,db.Clock).Rows("Faculty"));
        else
            Assert.NotEmpty(new AdminMaintenanceController(store,access,db.Clock).Rows("Faculty"));
    }
}

public sealed class NamedStaffAccountTests
{
    [LocalDbTheory]
    [InlineData("Adonias", "NKSADO001", UserRole.Administrator)]
    [InlineData("SIRDUBES", "MDBSI003", UserRole.Advisor)]
    public void ReadmeCredentialsSignInAfterReopeningDatabase(string username, string password, UserRole role)
    {
        using TestDatabase db = new();
        var reopened = new FuturePath.Data.SqlServerStore(db.Store.DatabaseName);
        reopened.Initialize();
        var access = new AccessSession();
        new AuthenticationController(reopened, access, db.Clock).Login(username, password);
        Assert.Equal(role, access.Role);
        Assert.Equal(username, access.DisplayName);
        Assert.NotEqual(password, reopened.FindUser(username)!.PasswordHash);
        Assert.Equal(10, new AdvisorySessionController(reopened, access, db.Clock).Search(DateOnly.MinValue, DateOnly.MaxValue).Count);
    }

    [LocalDbTheory]
    [InlineData(1)]
    [InlineData(2)]
    public void UpgradeAddsReadmeAccountsAndPreservesExistingAccountsAndLearners(int oldVersion)
    {
        using TestDatabase db = new();
        string oldHash = db.Store.FindUser("Adonias")!.PasswordHash;
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(db.Store.ConnectionString);
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "UPDATE dbo.UserAccount SET Username=N'legacy.admin' WHERE Username=N'Adonias'; UPDATE dbo.UserAccount SET Username=N'legacy.advisor' WHERE Username=N'SIRDUBES'; DELETE dbo.SchemaVersion WHERE Version>1; IF @version=2 INSERT dbo.SchemaVersion(Version) VALUES(2);";
            command.Parameters.AddWithValue("@version", oldVersion);
            command.ExecuteNonQuery();
        }
        db.Store.Initialize();
        db.Store.Initialize();
        Assert.Equal(oldHash, db.Store.FindUser("legacy.admin")!.PasswordHash);
        Assert.Equal(10, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
        db.Auth.Login("Adonias", "NKSADO001");
        Assert.Equal(UserRole.Administrator, db.Access.Role);
        db.Access.SignOut();
        db.Auth.Login("SIRDUBES", "MDBSI003");
        Assert.Equal(UserRole.Advisor, db.Access.Role);
        using var check = connection.CreateCommand();
        check.CommandText = "SELECT COUNT(*) FROM dbo.SchemaVersion WHERE Version=3";
        Assert.Equal(1, Convert.ToInt32(check.ExecuteScalar()));
        db.Auth.ChangeSecurity("SIRDUBES", "MDBSI003", "UpdatedPassword2026!");
        db.Store.Initialize();
        FuturePath.Data.DatabaseSetup.PrepareDemo(db.Store.DatabaseName);
        db.Access.SignOut();
        db.Auth.Login("SIRDUBES", "UpdatedPassword2026!");
        Assert.Equal(UserRole.Advisor, db.Access.Role);
    }
}
