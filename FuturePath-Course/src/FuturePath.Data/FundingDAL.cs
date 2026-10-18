using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.Data.SqlClient;
using FuturePath.Business;

namespace FuturePath.Data
{
    public class FundingDAL : DB
    {
        public FundingDAL(string databaseName = DatabaseConnection.DefaultDatabase) : base(databaseName)
        {
        }

        public List<FundingOption> GetFunding(int degreeId, DateOnly today)
        {
            using (SqlConnection c = Open())
            {
                // Explicit programme links narrow an award; faculty-wide options have no active programme links.
                using (SqlCommand cmd = Command(c, @"SELECT f.Id,f.Name,f.Type,f.Eligibility,f.Documents,f.ClosingDate,f.Contact,f.Sample
FROM FundingOption f JOIN DegreeProgramme d ON d.Id=@p0 JOIN Faculty fa ON fa.Id=d.FacultyId
WHERE f.Active=1 AND d.Active=1 AND fa.Active=1 AND f.ClosingDate>=@p1 AND (
  EXISTS(SELECT 1 FROM FundingProgramme fp WHERE fp.FundingId=f.Id AND fp.DegreeId=d.Id AND fp.Active=1)
  OR (NOT EXISTS(SELECT 1 FROM FundingProgramme fp WHERE fp.FundingId=f.Id AND fp.Active=1)
      AND EXISTS(SELECT 1 FROM FundingFaculty ff WHERE ff.FundingId=f.Id AND ff.FacultyId=d.FacultyId AND ff.Active=1)))
ORDER BY f.ClosingDate,f.Name", null, degreeId, today.ToDateTime(TimeOnly.MinValue)))
                {
                    DataTable tableData = ReadData(cmd);
                    List<FundingOption> result = new List<FundingOption>();
                    foreach (DataRow row in tableData.Rows)
                    {
                        result.Add(new FundingOption {
                            Id = Convert.ToInt32(row[0]),
                            Name = (Convert.ToString(row[1]) ?? ""),
                            Type = (Convert.ToString(row[2]) ?? ""),
                            Eligibility = (Convert.ToString(row[3]) ?? ""),
                            Documents = (Convert.ToString(row[4]) ?? ""),
                            ClosingDate = DateOnly.FromDateTime(Convert.ToDateTime(row[5])),
                            Contact = (Convert.ToString(row[6]) ?? ""),
                            Sample = Convert.ToBoolean(row[7])
                        });
                    }

                    return result;
                }
            }
        }
    }
}
