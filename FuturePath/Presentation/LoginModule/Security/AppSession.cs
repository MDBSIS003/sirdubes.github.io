namespace FuturePathStudentAdvisingSystem.Security
{
    /// <summary>
    /// Holds who is currently using the application (single-user system, so static is fine).
    /// Every form that needs a role restriction should check AppSession, e.g.
    /// if (!AppSession.IsInRole(Roles.Admin)) { ...deny... }
    /// </summary>
    public static class AppSession
    {
        public static int? UserID { get; private set; }
        public static string Role { get; private set; } = Roles.Guest;
        public static string DisplayName { get; private set; } = "Guest";

        public static bool IsLoggedIn => UserID != null;

        public static void StartGuest()
        {
            UserID = null;
            Role = Roles.Guest;
            DisplayName = "Guest";
        }

        public static void Start(int userID, string role, string displayName)
        {
            UserID = userID;
            Role = role;
            DisplayName = displayName;
        }

        public static void End() => StartGuest();

        public static bool IsInRole(string role) => Role == role;
    }
}
