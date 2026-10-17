namespace FuturePathStudentAdvisingSystem.Security
{
    /// <summary>
    /// Who is using the application right now (single-user system, so static is fine).
    /// Use AppSession.IsInRole(Roles.Admin) etc. to restrict screens by role.
    /// </summary>
    public static class AppSession
    {
        public static int? UserID { get; private set; }
        public static string Role { get; private set; } = Roles.Guest;
        public static string DisplayName { get; private set; } = "Guest";

        public static bool IsLoggedIn => UserID != null;

        public static void StartGuest()
        {
            FuturePathStudentAdvisingSystem.BusinessLogic.AdvisingWorkflow.Clear();
            UserID = null;
            Role = Roles.Guest;
            DisplayName = "Guest";
        }

        internal static void Start(int userID, string role, string displayName)
        {
            if (userID <= 0 || !new[] { Roles.Learner, Roles.Advisor, Roles.Admin }.Contains(role))
                throw new ArgumentException("Invalid authenticated session.");
            FuturePathStudentAdvisingSystem.BusinessLogic.AdvisingWorkflow.Clear();
            UserID = userID;
            Role = role;
            DisplayName = displayName;
        }

        public static void End() => StartGuest();

        public static bool IsInRole(string role) => Role == role;
    }
}
