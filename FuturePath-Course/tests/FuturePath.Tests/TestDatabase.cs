using FuturePath.Business;
using FuturePath.Data;

namespace FuturePath.Tests;
public sealed class TestClock : IClock
{
    public DateTime UtcNow { get; set; } = new(2026, 10, 17, 12, 0, 0, DateTimeKind.Utc);
    public DateOnly Today => DateOnly.FromDateTime(UtcNow);
}

public sealed class TestDatabase : IDisposable
{
    public LocalDbDatabase Database { get; }
    public string DirectoryPath => Database.DirectoryPath;
    public SqlServerStore Store { get; }
    public AccessSession Access { get; } = new();
    public TestClock Clock { get; } = new();
    public AdvisorySessionController Advising { get; }
    public AuthenticationController Auth { get; }
    public AdminMaintenanceController Maintenance { get; }
    public ReportController Reports { get; }

    public TestDatabase()
    {
        Database = new LocalDbDatabase();
        Store = Database.Store;
        Advising = new(Store, Access, Clock);
        Auth = new(Store, Access, Clock);
        Maintenance = new(Store, Access, Clock);
        Reports = new(Store, Access, Clock);
    }

    public void Admin() => Auth.Login("admin_test", "FuturePathAdmin2026!");
    public void Advisor() => Auth.Login("advisor_test", "FuturePathAdvisor2026!");
    public AdvisoryInput Input(int sampleId = 1)
    {
        var input = Store.GetSession(sampleId)!.Input;
        input.Learner.Id = 0;
        return input;
    }

    public void Dispose()
    {
        Database.Dispose();
    }
}
