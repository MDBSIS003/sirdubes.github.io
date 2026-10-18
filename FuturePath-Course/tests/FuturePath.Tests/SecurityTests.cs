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
        Assert.Throws<ValidationException>(() => db.Auth.Login("sidurbes", "incorrect"));
        Assert.Throws<ValidationException>(() => db.Auth.Login("' OR 1=1 --", "incorrect"));
        Assert.Equal(UserRole.Learner, db.Access.Role);
    }

    [LocalDbFact]
    public void FifthFailureLocksAccountAndExpiryAllowsSuccessfulLogin()
    {
        using var db = new TestDatabase();
        for (var i = 0; i < 5; i++)
            Assert.Throws<ValidationException>(() => db.Auth.Login("adonias", "incorrect"));
        Assert.NotNull(db.Store.FindUser("adonias")!.LockedUntilUtc);
        Assert.Throws<ValidationException>(() => db.Advisor());
        db.Clock.UtcNow = db.Clock.UtcNow.AddMinutes(6);
        db.Advisor();
        Assert.Equal(UserRole.Advisor, db.Access.Role);
        Assert.Equal(0, db.Store.FindUser("adonias")!.FailedAttempts);
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
        Assert.Throws<ValidationException>(() => db.Auth.SaveUser(db.Access.UserId!.Value, "sidurbes", UserRole.Administrator, false, null));
        Assert.Throws<ValidationException>(() => db.Auth.SaveUser(db.Access.UserId!.Value, "sidurbes", UserRole.Advisor, true, null));
    }

    [LocalDbFact]
    public void PasswordChangeVerifiesOldPasswordAndAppliesNewPassword()
    {
        using var db = new TestDatabase();
        db.Advisor();
        Assert.Throws<ValidationException>(() => db.Auth.ChangePassword("wrong", "ReplacementPass12"));
        db.Auth.ChangePassword("NKSADO001", "ReplacementPass12");
        db.Access.SignOut();
        Assert.Throws<ValidationException>(() => db.Advisor());
        db.Auth.Login("adonias", "ReplacementPass12");
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
        string password = replacePassword ? "ReplacementPass2026!" : "NKSADO001";
        db.Auth.ChangeSecurity("qa.advisor", "NKSADO001", replacePassword ? password : "");
        var store = new FuturePath.Data.SqlServerStore(db.Store.DatabaseName);
        var user = store.FindUser("qa.advisor")!;
        Assert.Equal(id,user.Id);
        Assert.Equal(UserRole.Advisor,user.Role);
        Assert.True(new PasswordHasher().Verify(password,user.PasswordHash));
        Assert.Null(store.FindUser("adonias"));
        db.Access.SignOut();
        db.Auth.Login("qa.advisor",password);
        Assert.Throws<AccessDeniedException>(() => db.Maintenance.Rows("Faculty"));
    }
    [LocalDbFact]
    public void DuplicateUsernameRollsBackPasswordAndLeavesBothAccountsUsable()
    {
        using TestDatabase db = new(); db.Advisor();
        Assert.Throws<ValidationException>(() => db.Auth.ChangeSecurity("SIDURBES","NKSADO001","ReplacementPass2026!"));
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
        Assert.Throws<Microsoft.Data.SqlClient.SqlException>(() => db.Auth.ChangeSecurity("qa.advisor","NKSADO001","ReplacementPass2026!"));
        Assert.Equal("adonias",db.Access.DisplayName);
        Assert.Null(db.Store.FindUser("qa.advisor"));
        Assert.True(new PasswordHasher().Verify("NKSADO001",db.Store.FindUser("adonias")!.PasswordHash));
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
        new AuthenticationController(reopened,access,db.Clock).Login("adonias","NKSADO001");
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
    [InlineData("sidurbes", "MDBSIS003", UserRole.Administrator)]
    [InlineData("adonias", "NKSADO001", UserRole.Advisor)]
    public void ReadmeCredentialsSignInAfterReopeningDatabase(string username, string password, UserRole role)
    {
        using TestDatabase db = new();
        var reopened = new FuturePath.Data.SqlServerStore(db.Store.DatabaseName);
        reopened.Initialize();
        var access = new AccessSession();
        new AuthenticationController(reopened, access, db.Clock).Login(username, password, role);
        Assert.Equal(role, access.Role);
        Assert.Equal(username, access.DisplayName);
        Assert.NotEqual(password, reopened.FindUser(username)!.PasswordHash);
        Assert.Equal(10, new AdvisorySessionController(reopened, access, db.Clock).Search(DateOnly.MinValue, DateOnly.MaxValue).Count);
    }

    [LocalDbTheory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void UpgradeAddsReadmeAccountsAndPreservesExistingAccountsAndLearners(int oldVersion)
    {
        using TestDatabase db = new();
        string oldHash = db.Store.FindUser("sidurbes")!.PasswordHash;
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(db.Store.ConnectionString);
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "UPDATE dbo.UserAccount SET Username=N'legacy.admin' WHERE Username=N'sidurbes'; UPDATE dbo.UserAccount SET Username=N'legacy.advisor' WHERE Username=N'adonias'; DELETE dbo.SchemaVersion WHERE Version>1; IF @version>1 INSERT dbo.SchemaVersion(Version) VALUES(@version);";
            command.Parameters.AddWithValue("@version", oldVersion);
            command.ExecuteNonQuery();
        }
        db.Store.Initialize();
        db.Store.Initialize();
        Assert.Equal(oldHash, db.Store.FindUser("legacy.admin")!.PasswordHash);
        Assert.Equal(10, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
        db.Auth.Login("sidurbes", "MDBSIS003", UserRole.Administrator);
        Assert.Equal(UserRole.Administrator, db.Access.Role);
        db.Access.SignOut();
        db.Auth.Login("adonias", "NKSADO001", UserRole.Advisor);
        Assert.Equal(UserRole.Advisor, db.Access.Role);
        using var check = connection.CreateCommand();
        check.CommandText = "SELECT COUNT(*) FROM dbo.SchemaVersion WHERE Version=4";
        Assert.Equal(1, Convert.ToInt32(check.ExecuteScalar()));
        db.Auth.ChangeSecurity("renamed.advisor", "NKSADO001", "UpdatedPassword2026!");
        db.Store.Initialize();
        FuturePath.Data.DatabaseSetup.PrepareDemo(db.Store.DatabaseName);
        db.Access.SignOut();
        db.Auth.Login("renamed.advisor", "UpdatedPassword2026!", UserRole.Advisor);
        Assert.Equal(UserRole.Advisor, db.Access.Role);
        Assert.Null(db.Store.FindUser("adonias"));
    }

    [LocalDbFact]
    public void VersionThreeUpgradeCorrectsRequestedRolesAndPasswordsOnceWithoutLosingOtherRecords()
    {
        using TestDatabase db = new();
        int advisorId = db.Store.FindUser("adonias")!.Id;
        int adminId = db.Store.FindUser("sidurbes")!.Id;
        db.Admin();
        db.Auth.SaveUser(0, "retained.staff", UserRole.Advisor, true, "RetainedPassword2026!");
        string retainedHash = db.Store.FindUser("retained.staff")!.PasswordHash;
        db.Access.SignOut();
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(db.Store.ConnectionString);
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            command.CommandText = "UPDATE dbo.UserAccount SET Username=N'Adonias', Role=N'Administrator', PasswordHash=@hash WHERE Id=@advisor; UPDATE dbo.UserAccount SET Username=N'SIRDUBES', Role=N'Advisor', PasswordHash=@hash WHERE Id=@admin; DELETE dbo.SchemaVersion WHERE Version>3; IF NOT EXISTS(SELECT 1 FROM dbo.SchemaVersion WHERE Version=3) INSERT dbo.SchemaVersion(Version) VALUES(3);";
            command.Parameters.AddWithValue("@hash", new PasswordHasher().Hash("PreviousPassword2026!"));
            command.Parameters.AddWithValue("@advisor", advisorId);
            command.Parameters.AddWithValue("@admin", adminId);
            command.ExecuteNonQuery();
        }
        db.Store.Initialize();
        Assert.Equal(advisorId, db.Store.FindUser("adonias")!.Id);
        Assert.Equal(adminId, db.Store.FindUser("sidurbes")!.Id);
        Assert.Null(db.Store.FindUser("SIRDUBES"));
        Assert.Equal(retainedHash, db.Store.FindUser("retained.staff")!.PasswordHash);
        Assert.Equal(10, db.Store.FindSessions(DateOnly.MinValue, DateOnly.MaxValue, "").Count);
        Assert.Throws<ValidationException>(() => db.Auth.Login("adonias", "NKSADO001", UserRole.Administrator));
        Assert.False(db.Access.IsStaff);
        db.Auth.Login("adonias", "NKSADO001", UserRole.Advisor);
        db.Access.SignOut();
        db.Auth.Login("sidurbes", "MDBSIS003", UserRole.Administrator);
        db.Auth.ChangeSecurity("renamed.admin", "MDBSIS003", "UpdatedAdminPassword2026!");
        db.Store.Initialize();
        FuturePath.Data.DatabaseSetup.PrepareDemo(db.Store.DatabaseName);
        db.Access.SignOut();
        db.Auth.Login("renamed.admin", "UpdatedAdminPassword2026!", UserRole.Administrator);
        Assert.Equal(UserRole.Administrator, db.Access.Role);
        Assert.Null(db.Store.FindUser("sidurbes"));
        Assert.Equal(retainedHash, db.Store.FindUser("retained.staff")!.PasswordHash);
    }
}
