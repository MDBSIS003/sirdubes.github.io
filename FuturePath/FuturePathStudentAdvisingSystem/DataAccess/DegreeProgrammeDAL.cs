using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FuturePathStudentAdvisingSystem.Models;

namespace FuturePathStudentAdvisingSystem.DataAccess
{
    public class DegreeProgrammeDAL
    {
        // Historical summaries must still identify a selected programme after deactivation.
        public DegreeProgramme? GetProgramme(int degreeID)
        {
            var table = SqlStore.Table("""
                SELECT d.DegreeID,d.FacultyID,d.ProgrammeName,d.Description,d.IsActive,f.FacultyName
                FROM dbo.DegreeProgramme d JOIN dbo.Faculty f ON f.FacultyID=d.FacultyID WHERE d.DegreeID=@id
                """, ("@id", degreeID));
            if (table.Rows.Count == 0) return null;
            var row = table.Rows[0];
            return new DegreeProgramme
            {
                DegreeID = (int)row["DegreeID"],
                FacultyID = (int)row["FacultyID"],
                ProgrammeName = (string)row["ProgrammeName"],
                Description = Convert.ToString(row["Description"])!,
                IsActive = (bool)row["IsActive"],
                FacultyName = (string)row["FacultyName"]
            };
        }

        public List<DegreeProgramme> GetActiveDegreeProgrammes()
        {
            var programmes = new List<DegreeProgramme>();

            const string query = @"
                SELECT
                    dp.DegreeID,
                    dp.FacultyID,
                    dp.ProgrammeName,
                    dp.Description,
                    dp.IsActive,
                    f.FacultyName
                FROM dbo.DegreeProgramme AS dp
                INNER JOIN dbo.Faculty AS f
                    ON dp.FacultyID = f.FacultyID
                WHERE dp.IsActive = 1
                ORDER BY dp.ProgrammeName;";

            using (SqlConnection connection =
                   DatabaseConnection.CreateConnection())
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var programme = new DegreeProgramme
                        {
                            DegreeID = reader.GetInt32(
                                reader.GetOrdinal("DegreeID")),

                            FacultyID = reader.GetInt32(
                                reader.GetOrdinal("FacultyID")),

                            ProgrammeName = reader.GetString(
                                reader.GetOrdinal("ProgrammeName")),

                            Description = reader.IsDBNull(
                                reader.GetOrdinal("Description"))
                                ? string.Empty
                                : reader.GetString(
                                    reader.GetOrdinal("Description")),

                            IsActive = reader.GetBoolean(
                                reader.GetOrdinal("IsActive")),

                            FacultyName = reader.GetString(
                                reader.GetOrdinal("FacultyName"))
                        };

                        programmes.Add(programme);
                    }
                }
            }

            return programmes;
        }
    }
}