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

    public void Admin() => Auth.Login("sidurbes", "MDBSIS003");
    public void Advisor() => Auth.Login("adonias", "NKSADO001");
    public AdvisoryInput Input(int sampleId = 1)
    {
        var input = Store.GetSession(sampleId)!.Input;
        input.Learner.Id = 0;
        input.Learner.Email = "learner@example.com";
        input.Learner.Phone = "0712345678";
        input.Learner.School = "QA School";
        input.Learner.Province = "Western Cape";
        var used = input.Marks.Select(m => m.SubjectId).ToHashSet();
        foreach (var subject in Store.GetLookup("Subject"))
        {
            if (input.Marks.Count >= AdvisingValidation.MinimumSubjects) { break; }
            if (subject.Id == 3 || subject.Id == 5 || !used.Add(subject.Id)) { continue; }
            input.Marks.Add(new SubjectMark(subject.Id, subject.Name, 70));
        }
        return input;
    }

    public void Dispose()
    {
        Database.Dispose();
    }
}
