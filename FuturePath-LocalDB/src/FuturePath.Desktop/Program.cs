using System.Security.Cryptography;
using System.Text;
using FuturePath.Business;
using FuturePath.Data;

namespace FuturePath.Desktop;
internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => Ui.ShowError(e.Exception);
        try
        {
            var databaseName = DatabaseConnection.DefaultDatabase;
            if (args.Length > 0)
            {
                if (args.Length != 2 || args[0] != "--database")
                    throw new ValidationException("Use --database LocalDatabaseName, or launch without arguments.");
                databaseName = args[1];
            }

            var suffix = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(databaseName.ToUpperInvariant())))[..20];
            using var mutex = new Mutex(true, "Local\\FuturePath-" + suffix, out var firstInstance);
            if (!firstInstance)
            {
                MessageBox.Show("Future Path is already open for this database. Switch to the existing window.", "Future Path");
                return;
            }

            var store = new SqlServerStore(databaseName);
            Application.Run(new MainForm(new AppServices(store), store));
        }
        catch (Exception ex)
        {
            Ui.ShowError(ex);
        }
    }
}

internal sealed class AppServices
{
    public AccessSession Access { get; } = new();
    public AdvisingService Advising { get; }
    public AuthenticationService Authentication { get; }
    public MaintenanceService Maintenance { get; }
    public ReportService Reports { get; }
    public string DatabaseName { get; }

    public AppServices(SqlServerStore store)
    {
        var clock = new SystemClock();
        DatabaseName = store.DatabaseName;
        Advising = new(store, Access, clock);
        Authentication = new(store, Access, clock);
        Maintenance = new(store, Access, clock);
        Reports = new(store, Access, clock);
    }
}
