using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using FuturePath.Business;

namespace FuturePath.Data
{
         public sealed class DatabaseConnection
    {
        private string databaseName = "";
        private string connectionString = "";
        public const string DefaultDatabase = "FuturePathStudentAdvisingSystem";
        public string DatabaseName
        {
            get { return databaseName; }
        }

        public string ConnectionString
        {
            get { return connectionString; }
        }

        public DatabaseConnection(string databaseName = DefaultDatabase)
        {
            if (!Regex.IsMatch(databaseName, "^[A-Za-z][A-Za-z0-9_]{0,127}$"))
            {
                throw new ValidationException("Use a database name containing letters, numbers and underscores, not a database file path.");
            }

            string[] systemNames =
            {
                "master",
                "model",
                "msdb",
                "tempdb"
            };
            foreach (string name in systemNames)
            {
                if (string.Equals(name, databaseName, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ValidationException("Choose a separate application database, not a SQL Server system database.");
                }
            }

            this.databaseName = databaseName;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = databaseName,
                IntegratedSecurity = true,
                TrustServerCertificate = true,
                ConnectTimeout = 15,
                ApplicationName = "FuturePath"
            };
            connectionString = builder.ConnectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
