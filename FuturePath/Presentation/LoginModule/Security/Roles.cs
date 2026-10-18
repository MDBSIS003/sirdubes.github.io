namespace FuturePathStudentAdvisingSystem.Security
{
    /// <summary>
    /// Role names. The three database roles MUST match the CHECK constraint on
    /// UserAccount.UserRole exactly. "Guest" is never stored in the database.
    /// </summary>
    public static class Roles
    {
        public const string Guest = "Guest";
        public const string Learner = "Learner";
        public const string Advisor = "Career Advisor";
        public const string Admin = "System Administrator";
    }
}
