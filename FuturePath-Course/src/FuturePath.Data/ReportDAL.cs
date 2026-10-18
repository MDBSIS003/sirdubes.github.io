using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using Microsoft.Data.SqlClient;
using FuturePath.Business;

namespace FuturePath.Data
{
    public class ReportDAL : DB
    {
        public ReportDAL(string databaseName = DatabaseConnection.DefaultDatabase) : base(databaseName)
        {
        }

        public void Audit(int? userId, string action, string detail, DateTime now)
        {
            using (SqlConnection c = Open())
            {
                Execute(c, "INSERT INTO AuditEvent(UserId,Action,Detail,CreatedUtc) VALUES(@p0,@p1,@p2,@p3)", null, userId, action, detail, now);
            }
        }

        public void RecordExport(int sessionId, string path, DateTime now)
        {
            using (SqlConnection c = Open())
            {
                Execute(c, "INSERT INTO ExportReport(SessionId,Path,GeneratedUtc) VALUES(@p0,@p1,@p2)", null, sessionId, path, now);
            }
        }

        public void Backup(string path)
        {
            path = Path.GetFullPath(path);
            if (!Path.GetExtension(path).Equals(".bak", StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidationException("Choose a new SQL Server backup filename ending in .bak.");
            }

            if (File.Exists(path))
            {
                throw new ValidationException("Choose a new backup filename to preserve existing backups.");
            }

            using (SqlConnection connection = Open())
            {
                using (SqlCommand command = Command(connection, "BACKUP DATABASE [" + DatabaseName + "] TO DISK=@p0 WITH COPY_ONLY,CHECKSUM; RESTORE VERIFYONLY FROM DISK=@p0 WITH CHECKSUM;", null, path))
                {
                    command.CommandTimeout = 120;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
