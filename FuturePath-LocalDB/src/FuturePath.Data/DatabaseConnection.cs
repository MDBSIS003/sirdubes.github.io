using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using FuturePath.Business;

namespace FuturePath.Data;
/// <summary>One local SQL Server instance, Windows authentication, no stored password.</summary>
public sealed class DatabaseConnection
{
    public const string DefaultDatabase = "FuturePathStudentAdvisingSystem";
    public string DatabaseName { get; }
    public string ConnectionString { get; }

    public DatabaseConnection(string databaseName = DefaultDatabase)
    {
        if (!Regex.IsMatch(databaseName, "^[A-Za-z][A-Za-z0-9_]{0,127}$"))
            throw new ValidationException("Use a database name containing letters, numbers and underscores, not a database file path.");
        if (new[]
        {
            "master",
            "model",
            "msdb",
            "tempdb"
        }.Contains(databaseName, StringComparer.OrdinalIgnoreCase))
            throw new ValidationException("Choose a separate application database, not a SQL Server system database.");
        DatabaseName = databaseName;
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = @"(localdb)\MSSQLLocalDB",
            InitialCatalog = databaseName,
            IntegratedSecurity = true,
            TrustServerCertificate = true,
            ConnectTimeout = 15,
            ApplicationName = "FuturePath"
        };
        ConnectionString = builder.ConnectionString;
    }

    public SqlConnection CreateConnection() => new(ConnectionString);
}
