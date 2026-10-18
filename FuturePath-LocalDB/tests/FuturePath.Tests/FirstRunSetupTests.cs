using FuturePath.Data;
using Microsoft.Data.SqlClient;

namespace FuturePath.Tests;

public sealed class FirstRunSetupTests
{
    [LocalDbFact]
    public void EmbeddedSetupCreatesMissingDatabaseAndPreservesChangesOnRepeat()
    {
        using var db = new LocalDbDatabase(seed: false, createDatabase: false);
        DatabaseSetup.PrepareDemo(db.DatabaseName);
        db.Store.Initialize();
        Assert.Equal(18, db.Store.GetLookup("Subject").Count);
        using var connection = new SqlConnection(db.Store.ConnectionString);
        connection.Open();
        using (var edit = new SqlCommand("UPDATE dbo.Learner SET Name=N'Retained learner' WHERE Id=1; UPDATE dbo.UserAccount SET PasswordHash=N'Preserved test hash' WHERE Id=1;", connection))
            edit.ExecuteNonQuery();

        DatabaseSetup.PrepareDemo(db.DatabaseName);
        using var verify = new SqlCommand("SELECT (SELECT Name FROM dbo.Learner WHERE Id=1), (SELECT PasswordHash FROM dbo.UserAccount WHERE Id=1), (SELECT COUNT(*) FROM dbo.AdvisorySession)", connection);
        using var reader = verify.ExecuteReader();
        Assert.True(reader.Read());
        Assert.Equal("Retained learner", reader.GetString(0));
        Assert.Equal("Preserved test hash", reader.GetString(1));
        Assert.Equal(10, reader.GetInt32(2));
    }

    [LocalDbFact]
    public void EmbeddedSetupRefusesAnUnrelatedDatabaseWithoutChangingItsTable()
    {
        using var db = new LocalDbDatabase(seed: false);
        using var connection = new SqlConnection(db.Store.ConnectionString);
        connection.Open();
        using (var create = new SqlCommand("CREATE TABLE dbo.Unrelated (Id INT PRIMARY KEY); INSERT dbo.Unrelated VALUES(42);", connection))
            create.ExecuteNonQuery();
        var exception = Assert.Throws<SqlException>(() => DatabaseSetup.PrepareDemo(db.DatabaseName));
        Assert.Equal(51000, exception.Number);
        using var verify = new SqlCommand("SELECT Id FROM dbo.Unrelated", connection);
        Assert.Equal(42, (int)verify.ExecuteScalar()!);
    }
}
