using System.Data;
using Microsoft.Data.SqlClient;

namespace FuturePathStudentAdvisingSystem.DataAccess
{
    public class SubjectDAL
    {
        public DataTable GetSubjects()
        {
            DataTable subjects = new DataTable();

            using (SqlConnection connection = DatabaseConnection.CreateConnection())
            {
                string query = "SELECT SubjectID, SubjectName FROM dbo.Subject ORDER BY SubjectName";

                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    adapter.Fill(subjects);
                }
            }

            return subjects;
        }
    }
}