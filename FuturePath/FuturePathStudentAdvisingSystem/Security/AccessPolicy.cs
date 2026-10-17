namespace FuturePathStudentAdvisingSystem.Security;

public static class AccessPolicy
{
    public static void RequireRole(params string[] roles)
    {
        if (!AppSession.IsLoggedIn || !roles.Contains(AppSession.Role))
            throw new UnauthorizedAccessException("This operation is not authorised for your account.");
    }
    public static void RequireLearner(int userID)
    {
        RequireRole(Roles.Learner);
        if (AppSession.UserID != userID) throw new UnauthorizedAccessException("Only your own learner record may be changed.");
    }
    public static void RequireStaff() => RequireRole(Roles.Advisor, Roles.Admin);
    public static void RequirePage(string page)
    {
        if (page is "Advisor") RequireRole(Roles.Advisor);
        else if (page is "Administrator") RequireRole(Roles.Admin);
        else if (page is "Reports") RequireStaff();
        else if (AppSession.Role != Roles.Guest) RequireRole(Roles.Learner);
    }
}
