using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using FuturePath.Data;

namespace FuturePath.TestSupport;
public sealed class LocalDbFactAttribute : Xunit.FactAttribute
{
    public LocalDbFactAttribute()
    {
        if (!OperatingSystem.IsWindows())
            Skip = "Requires Windows SQL Server LocalDB; no substitute database is used.";
    }
}

public sealed class LocalDbTheoryAttribute : Xunit.TheoryAttribute
{
    public LocalDbTheoryAttribute()
    {
        if (!OperatingSystem.IsWindows())
            Skip = "Requires Windows SQL Server LocalDB; no substitute database is used.";
    }
}

/// <summary>Creates/removes only a uniquely named disposable test database, never the application database.</summary>
public sealed class LocalDbDatabase : IDisposable
{
    public string DatabaseName { get; } = "FuturePathTest_" + Guid.NewGuid().ToString("N");
    public string DirectoryPath { get; } = Path.Combine(Path.GetTempPath(), "FuturePathLocalDbTests", Guid.NewGuid().ToString("N"));
    public SqlServerStore Store { get; }
    private string MasterConnection => new SqlConnectionStringBuilder(Store.ConnectionString)
    {
        InitialCatalog = "master"
    }.ConnectionString;

    public LocalDbDatabase(bool seed = true, bool createDatabase = true)
    {
        Store = new SqlServerStore(DatabaseName);
        Directory.CreateDirectory(DirectoryPath);
        try
        {
            if (createDatabase)
                Execute("CREATE DATABASE [" + DatabaseName + "]", MasterConnection);
            if (seed)
                ApplySetupAgain();
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    public void ApplySetupAgain()
    {
        foreach (string file in new[]
        {
            "02_CreateTables.sql",
            "03_SeedData.sql",
            "04_VerifyDatabase.sql"
        }

        )
        {
            string script = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Database", file));
            script = script.Replace(DatabaseConnection.DefaultDatabase, DatabaseName, StringComparison.Ordinal);
            foreach (string batch in Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                if (!string.IsNullOrWhiteSpace(batch))
                    Execute(batch, Store.ConnectionString);
        }

        Store.Initialize();
    }

    public void RestoreBackup(string path)
    {
        string dataName = "", logName = "";
        using (var connection = new SqlConnection(MasterConnection))
        {
            connection.Open();
            using var command = new SqlCommand("RESTORE FILELISTONLY FROM DISK=@file", connection);
            command.Parameters.AddWithValue("@file", path);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(reader.GetOrdinal("Type")) == "D")
                    dataName = reader.GetString(reader.GetOrdinal("LogicalName"));
                if (reader.GetString(reader.GetOrdinal("Type")) == "L")
                    logName = reader.GetString(reader.GetOrdinal("LogicalName"));
            }
        }

        string Quote(string value) => "N'" + value.Replace("'", "''") + "'";
        string sql = "RESTORE DATABASE [" + DatabaseName + "] FROM DISK=" + Quote(path) + " WITH REPLACE, MOVE " + Quote(dataName) + " TO " + Quote(Path.Combine(DirectoryPath, "restored.mdf")) + ", MOVE " + Quote(logName) + " TO " + Quote(Path.Combine(DirectoryPath, "restored_log.ldf")) + ", CHECKSUM;";
        // WITH REPLACE applies exclusively to the empty, uniquely named test database created by this helper.
        Execute(sql, MasterConnection);
        Store.Initialize();
    }

    private static void Execute(string sql, string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        using var command = new SqlCommand(sql, connection)
        {
            CommandTimeout = 120
        };
        command.ExecuteNonQuery();
    }

    public void Dispose()
    {
        if (!DatabaseName.StartsWith("FuturePathTest_", StringComparison.Ordinal) || !Guid.TryParseExact(DatabaseName[15..], "N", out _))
            throw new InvalidOperationException("Refusing to remove a database outside the disposable test naming convention.");
        SqlConnection.ClearAllPools();
        Execute("IF DB_ID(N'" + DatabaseName + "') IS NOT NULL BEGIN ALTER DATABASE [" + DatabaseName + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [" + DatabaseName + "]; END;", MasterConnection);
        if (Directory.Exists(DirectoryPath))
            Directory.Delete(DirectoryPath, true);
    }
}
