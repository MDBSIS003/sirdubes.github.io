using System;
using Microsoft.Data.SqlClient;

namespace FuturePathStudentAdvisingSystem.DataAccess
{
    public class LearnerProfileDAL
    {
        public int SaveLearnerProfile(
            string firstName,
            string lastName,
            string email,
            string schoolName,
            string province)
        {
            string query = @"
                INSERT INTO dbo.UserAccount
                (Username, [Password], UserRole, FirstName,
                 LastName, SchoolName, Province)
                OUTPUT INSERTED.UserID
                VALUES
                (@Username, @Password, @UserRole, @FirstName,
                 @LastName, @SchoolName, @Province);";

            using (SqlConnection connection =
                   DatabaseConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", email);
                command.Parameters.AddWithValue(
                    "@Password", Guid.NewGuid().ToString("N"));
                command.Parameters.AddWithValue("@UserRole", "Learner");
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@SchoolName", schoolName);
                command.Parameters.AddWithValue("@Province", province);

                connection.Open();

                int userID = Convert.ToInt32(command.ExecuteScalar());

                return userID;
            }
        }
    }
}