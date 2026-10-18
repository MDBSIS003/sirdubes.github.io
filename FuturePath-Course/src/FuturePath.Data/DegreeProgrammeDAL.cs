using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.Data.SqlClient;
using FuturePath.Business;

namespace FuturePath.Data
{
    public class DegreeProgrammeDAL : DB
    {
        public DegreeProgrammeDAL(string databaseName = DatabaseConnection.DefaultDatabase) : base(databaseName)
        {
        }

        public List<DegreeProgramme> GetProgrammes(bool activeOnly)
        {
            using (SqlConnection c = Open())
            {
                List<DegreeProgramme> result = new List<DegreeProgramme>();
                using (SqlCommand cmd = Command(c, "SELECT d.Id,d.FacultyId,f.Name,d.Name,d.Description,d.Active,d.MinimumAverage FROM DegreeProgramme d JOIN Faculty f ON f.Id=d.FacultyId" + (activeOnly ? " WHERE d.Active=1 AND f.Active=1" : "") + " ORDER BY d.Name"))
                {
                    foreach (DataRow row in ReadData(cmd).Rows)
                    {
                        result.Add(new DegreeProgramme {
                            Id = Convert.ToInt32(row[0]),
                            FacultyId = Convert.ToInt32(row[1]),
                            Faculty = (Convert.ToString(row[2]) ?? ""),
                            Name = (Convert.ToString(row[3]) ?? ""),
                            Description = (Convert.ToString(row[4]) ?? ""),
                            Active = Convert.ToBoolean(row[5]),
                            MinimumAverage = Convert.ToDecimal(row[6])
                        });
                    }
                }

                foreach (DegreeProgramme p in result)
                {
                    using (SqlCommand cmd = Command(c, "SELECT q.SubjectId,s.Name,q.MinimumMark,q.Mandatory,q.AlternativeGroup,q.Note FROM ProgrammeRequirement q JOIN Subject s ON s.Id=q.SubjectId WHERE q.DegreeId=@p0 AND q.Active=1 ORDER BY q.Id", null, p.Id))
                    {
                        foreach (DataRow row in ReadData(cmd).Rows)
                        {
                            p.Requirements.Add(new ProgrammeRequirement {
                                SubjectId = Convert.ToInt32(row[0]),
                                Subject = (Convert.ToString(row[1]) ?? ""),
                                MinimumMark = Convert.ToDecimal(row[2]),
                                Mandatory = Convert.ToBoolean(row[3]),
                                AlternativeGroup = Convert.ToInt32(row[4]),
                                Note = (Convert.ToString(row[5]) ?? "")
                            });
                        }
                    }

                    using (SqlCommand cmd = Command(c, "SELECT CategoryId,Weight FROM DegreeInterestMapping WHERE DegreeId=@p0 AND Active=1 ORDER BY CategoryId", null, p.Id))
                    {
                        foreach (DataRow row in ReadData(cmd).Rows)
                        {
                            p.Interests.Add(new InterestWeight {
                                CategoryId = Convert.ToInt32(row[0]),
                                Weight = Convert.ToDecimal(row[1])
                            });
                        }
                    }
                }

                return result;
            }
        }
    }
}
