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
                using (SqlCommand cmd = Command(c, "SELECT u.Id,u.Username,u.PasswordHash,u.Role,u.Active,u.FailedAttempts,u.LockedUntilUtc FROM UserAccount u LEFT JOIN TesterRegistration t ON t.UserId=u.Id WHERE u.Username=@p0 OR t.Email=@p0", null, username))
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

        public UserAccount CreateTesterAccount(string email, string username, string hash, UserRole role, DateTime now)
        {
            using (SqlConnection c = Open())
            using (SqlTransaction tx = c.BeginTransaction())
            {
                try
                {
                    int id;
                    using (SqlCommand command = Command(c, "INSERT INTO UserAccount(Username,PasswordHash,Role,Active) OUTPUT INSERTED.Id VALUES(@p0,@p1,@p2,1)", tx, username, hash, role.ToString()))
                    {
                        id = Convert.ToInt32(command.ExecuteScalar());
                    }
                    Execute(c, "INSERT INTO TesterRegistration(UserId,Email,CreatedUtc) VALUES(@p0,@p1,@p2)", tx, id, email, now);
                    Execute(c, "INSERT INTO AuditEvent(UserId,Action,Detail,CreatedUtc) VALUES(@p0,@p1,@p2,@p3)", tx,
                        id, "Offline tester account created", username, now);
                    tx.Commit();
                    return new UserAccount { Id = id, Username = username, PasswordHash = hash, Role = role, Active = true };
                }
                catch (SqlException ex) when (ex.Number is 2601 or 2627)
                {
                    throw new ValidationException("That username or tester email is already registered. Sign in or use different account details.");
                }
            }
        }

        public void SaveSecurity(int userId, string username, string? passwordHash, DateTime now)
        {
            using (SqlConnection c = Open())
            using (SqlTransaction tx = c.BeginTransaction())
            {
                try
                {
                    int changed = Execute(c, @"UPDATE UserAccount SET Username=@p1,
PasswordHash=COALESCE(@p2,PasswordHash),FailedAttempts=0,LockedUntilUtc=NULL
WHERE Id=@p0 AND Active=1 AND Role IN ('Advisor','Administrator')", tx, userId, username, passwordHash);
                    if (changed != 1)
                    {
                        throw new AccessDeniedException();
                    }
                    Execute(c, "INSERT INTO AuditEvent(UserId,Action,Detail,CreatedUtc) VALUES(@p0,@p1,@p2,@p3)",
                        tx, userId, "Security settings updated", username, now);
                    tx.Commit();
                }
                catch (SqlException ex) when (ex.Number is 2601 or 2627)
                {
                    throw new ValidationException("That username is already in use. Choose another username.");
                }
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
