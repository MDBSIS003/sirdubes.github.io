using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace FuturePath.Data
{
         public static class DatabaseSetup
    {
        public static void PrepareDemo(string databaseName = DatabaseConnection.DefaultDatabase)
        {

            DatabaseConnection database = new DatabaseConnection(databaseName);
            using (SqlConnection connection = new SqlConnection(new SqlConnectionStringBuilder(database.ConnectionString) { InitialCatalog = "master" }.ConnectionString))
            {
                connection.Open();
                foreach (string file in new[]
                {
                    "01_CreateDatabase.sql",
                    "02_CreateTables.sql",
                    "03_SeedData.sql"
                }

                )
                {
                    using (Stream stream = typeof(DatabaseSetup).Assembly.GetManifestResourceStream("FuturePath.Setup." + file) ?? throw new InvalidOperationException("The application is missing its database setup resource: " + file))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string script = reader.ReadToEnd().Replace(DatabaseConnection.DefaultDatabase, database.DatabaseName, StringComparison.Ordinal);

                            foreach (string batch in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                            {
                                if (string.IsNullOrWhiteSpace(batch))
                                {
                                    continue;
                                }

                                using (SqlCommand command = new SqlCommand(batch, connection)
                                {
                                    CommandTimeout = 90
                                }

                                )
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }

                new SqlServerStore(database.DatabaseName).Initialize();
            }
        }
    }
}
