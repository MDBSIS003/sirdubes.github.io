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
        Assert.Throws<ValidationException>(() => db.Auth.Login("admin_test", "incorrect"));
        Assert.Throws<ValidationException>(() => db.Auth.Login("' OR 1=1 --", "incorrect"));
        Assert.Equal(UserRole.Learner, db.Access.Role);
    }

    [LocalDbFact]
    public void FifthFailureLocksAccountAndExpiryAllowsSuccessfulLogin()
    {
        using var db = new TestDatabase();
        for (var i = 0; i < 5; i++)
            Assert.Throws<ValidationException>(() => db.Auth.Login("advisor_test", "incorrect"));
        Assert.NotNull(db.Store.FindUser("advisor_test")!.LockedUntilUtc);
        Assert.Throws<ValidationException>(() => db.Advisor());
        db.Clock.UtcNow = db.Clock.UtcNow.AddMinutes(6);
        db.Advisor();
        Assert.Equal(UserRole.Advisor, db.Access.Role);
        Assert.Equal(0, db.Store.FindUser("advisor_test")!.FailedAttempts);
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
        Assert.Throws<ValidationException>(() => db.Auth.SaveUser(db.Access.UserId!.Value, "admin_test", UserRole.Administrator, false, null));
        Assert.Throws<ValidationException>(() => db.Auth.SaveUser(db.Access.UserId!.Value, "admin_test", UserRole.Advisor, true, null));
    }

    [LocalDbFact]
    public void PasswordChangeVerifiesOldPasswordAndAppliesNewPassword()
    {
        using var db = new TestDatabase();
        db.Advisor();
        Assert.Throws<ValidationException>(() => db.Auth.ChangePassword("wrong", "ReplacementPass12"));
        db.Auth.ChangePassword("FuturePathAdvisor2026!", "ReplacementPass12");
        db.Access.SignOut();
        Assert.Throws<ValidationException>(() => db.Advisor());
        db.Auth.Login("advisor_test", "ReplacementPass12");
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
