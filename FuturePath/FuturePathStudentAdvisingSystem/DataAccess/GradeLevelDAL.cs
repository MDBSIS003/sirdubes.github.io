using System.Data;
using Microsoft.Data.SqlClient;

namespace FuturePathStudentAdvisingSystem.DataAccess
{
    public class GradeLevelDAL
    {
        public DataTable GetGradeLevels()
        {
            const string query = @"
                SELECT GradeLevelID, GradeName
                FROM dbo.GradeLevel
                WHERE GradeName IN ('Grade 11','Grade 12')
                ORDER BY GradeLevelID;";

            DataTable grades = new DataTable();

            using (SqlConnection connection =
                   DatabaseConnection.CreateConnection())
            using (SqlDataAdapter adapter =
                   new SqlDataAdapter(query, connection))
            {
                adapter.Fill(grades);
            }

            return grades;
        }
    }
}