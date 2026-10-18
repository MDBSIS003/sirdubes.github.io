using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace FuturePath.Data;

/// <summary>Explicit, non-destructive demonstration setup, requested from the first-run screen.</summary>
public static class DatabaseSetup
{
    public static void PrepareDemo(string databaseName = DatabaseConnection.DefaultDatabase)
    {
        // Validate before substituting the catalog identifier in the trusted, embedded scripts.
        var database = new DatabaseConnection(databaseName);
        using var connection = new SqlConnection(new SqlConnectionStringBuilder(database.ConnectionString)
        {
            InitialCatalog = "master"
        }.ConnectionString);
        connection.Open();
        foreach (string file in new[] { "01_CreateDatabase.sql", "02_CreateTables.sql", "03_SeedData.sql" })
        {
            using var stream = typeof(DatabaseSetup).Assembly.GetManifestResourceStream("FuturePath.Setup." + file)
                ?? throw new InvalidOperationException("The application is missing its database setup resource: " + file);
            using var reader = new StreamReader(stream);
            string script = reader.ReadToEnd().Replace(DatabaseConnection.DefaultDatabase, database.DatabaseName, StringComparison.Ordinal);
            // GO is a script batch separator; keep one connection so seed transaction/SET state is retained.
            foreach (string batch in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(batch))
                    continue;
                using var command = new SqlCommand(batch, connection) { CommandTimeout = 90 };
                command.ExecuteNonQuery();
            }
        }
        new SqlServerStore(database.DatabaseName).Initialize();
    }
}
