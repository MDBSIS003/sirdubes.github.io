using Microsoft.Data.SqlClient;

namespace FuturePathStudentAdvisingSystem.DataAccess
{
    /// <summary>
    /// NOTE: Person 2's design also lists a UserAccountDAL. If they add one, merge this
    /// TryLogin method into theirs instead of keeping two classes with the same name.
    /// </summary>
    public class UserAccountDAL
    {
        /// <summary>
        /// Returns true when the username + password + role combination exists.
        /// The password comparison is case-sensitive (the database default collation is not).
        /// </summary>
        public bool TryLogin(string username, string password, string role,
                             out int userID, out string displayName)
        {
            const string query = @"
                SELECT UserID, FirstName, LastName
                FROM dbo.UserAccount
                WHERE Username = @Username
                  AND [Password] COLLATE Latin1_General_CS_AS = @Password
                  AND UserRole = @UserRole;";

            userID = 0;
            displayName = string.Empty;

            using (SqlConnection connection = DatabaseConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@UserRole", role);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return false;

                    userID = reader.GetInt32(0);
                    displayName = reader.GetString(1) + " " + reader.GetString(2);
                    return true;
                }
            }
        }
    }
}
