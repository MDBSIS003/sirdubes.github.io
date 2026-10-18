using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FuturePath.Business;
using FuturePath.Data;

namespace FuturePath.Desktop
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            try
            {
                string databaseName = DatabaseConnection.DefaultDatabase;
                if (args.Length > 0)
                {
                    if (args.Length != 2 || args[0] != "--database")
                    {
                        throw new ValidationException("Use --database LocalDatabaseName, or launch without arguments.");
                    }

                    databaseName = args[1];
                }

                // A database identifier is validated before it reaches SQL or the single-instance name.
                SqlServerStore store = new SqlServerStore(databaseName);
                string hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(databaseName.ToUpperInvariant())));
                bool firstInstance;
                using (Mutex mutex = new Mutex(true, "Local\\FuturePath-" + hash.Substring(0, 20), out firstInstance))
                {
                    if (!firstInstance)
                    {
                        MessageBox.Show("Future Path is already open for this database.", "Future Path");
                        return;
                    }

                    AppController app = new AppController(store);
                    Application.Run(new frmMain(app));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Future Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Future Path", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
