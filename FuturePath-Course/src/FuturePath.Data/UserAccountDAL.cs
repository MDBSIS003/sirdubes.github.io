using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.Data.SqlClient;
using FuturePath.Business;

namespace FuturePath.Data
{
    public class UserAccountDAL : DB
    {
        public UserAccountDAL(string databaseName = DatabaseConnection.DefaultDatabase) : base(databaseName)
        {
        }

        private static UserAccount ReadUser(DataRow row)
        {
            return new UserAccount()
            {
                Id = Convert.ToInt32(row[0]),
                Username = (Convert.ToString(row[1]) ?? ""),
                PasswordHash = (Convert.ToString(row[2]) ?? ""),
                Role = Enum.Parse<UserRole>((Convert.ToString(row[3]) ?? "")),
                Active = Convert.ToBoolean(row[4]),
                FailedAttempts = Convert.ToInt32(row[5]),
                LockedUntilUtc = row.IsNull(6) ? null : Utc(Convert.ToDateTime(row[6]))
            };
        }

        public UserAccount? FindUser(string username)
        {
            using (SqlConnection c = Open())
            {
                using (SqlCommand cmd = Command(c, "SELECT * FROM UserAccount WHERE Username=@p0", null, username))
                {
                    DataTable tableData = ReadData(cmd);
                    if (tableData.Rows.Count == 0)
                    {
                        return null;
                    }

                    return ReadUser(tableData.Rows[0]);
                }
            }
        }

        public UserAccount? GetUser(int id)
        {
            using (SqlConnection c = Open())
            {
                using (SqlCommand cmd = Command(c, "SELECT * FROM UserAccount WHERE Id=@p0", null, id))
                {
                    DataTable tableData = ReadData(cmd);
                    if (tableData.Rows.Count == 0)
                    {
                        return null;
                    }

                    return ReadUser(tableData.Rows[0]);
                }
            }
        }

        public void RecordLogin(int userId, bool success, DateTime now)
        {
            using (SqlConnection c = Open())
            {
                Execute(c, @"UPDATE UserAccount SET FailedAttempts=CASE WHEN @p1=1 OR LockedUntilUtc<=@p2 THEN 0 ELSE FailedAttempts END WHERE Id=@p0;
UPDATE UserAccount SET FailedAttempts=CASE WHEN @p1=1 THEN 0 ELSE FailedAttempts+1 END,
 LockedUntilUtc=CASE WHEN @p1=1 THEN NULL WHEN FailedAttempts>=4 THEN @p3 ELSE NULL END WHERE Id=@p0;", null, userId, success, now, now.AddMinutes(5));
            }
        }

        public void SavePassword(int userId, string hash)
        {
            using (SqlConnection c = Open())
            {
                Execute(c, "UPDATE UserAccount SET PasswordHash=@p1,FailedAttempts=0,LockedUntilUtc=NULL WHERE Id=@p0", null, userId, hash);
            }
        }

        public void SaveUser(int id, string username, UserRole role, bool active, string? passwordHash)
        {
            using (SqlConnection c = Open())
            {
                try
                {
                    if (id == 0)
                    {
                        Execute(c, "INSERT INTO UserAccount(Username,PasswordHash,Role,Active) VALUES(@p0,@p1,@p2,@p3)", null, username, passwordHash, role.ToString(), active);
                    }
                    else if (Execute(c, "UPDATE UserAccount SET Username=@p0,Role=@p1,Active=@p2,PasswordHash=COALESCE(@p3,PasswordHash),FailedAttempts=0,LockedUntilUtc=NULL WHERE Id=@p4", null, username, role.ToString(), active, passwordHash, id) == 0)
                    {
                        throw new ValidationException("Account not found.");
                    }
                }
                catch (SqlException ex)when (ex.Number is 2601 or 2627 or 547 or 515 or 2628)
                {
                    throw new ValidationException("An account with that username already exists, or the account values are invalid.");
                }
            }
        }
    }
}
