using Microsoft.Data.SqlClient;

namespace FuturePathStudentAdvisingSystem.DataAccess
{
    public static class DatabaseConnection
    {
        private const string ConnectionString =
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FuturePathAdvisingSystem;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=15;";

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(Environment.GetEnvironmentVariable("FUTUREPATH_CONNECTION_STRING") ?? ConnectionString);
        }

        public static bool TestConnection(out string message)
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    connection.Open();
                }

                message = "Connected to Future Path database successfully.";
                return true;
            }
            catch (SqlException ex)
            {
                message = "Database connection failed: " + ex.Message;
                return false;
            }
        }
    }
}