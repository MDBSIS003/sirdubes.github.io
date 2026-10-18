using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmDatabaseSetup : frmStaffForm
    {
        public frmDatabaseSetup()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        public frmDatabaseSetup(AppController app, string error) : this()
        {
            this.app = app;
            txtInstructions.Text = error + Environment.NewLine + Environment.NewLine + "Install SQL Server Express LocalDB using Visual Studio Installer > Modify > Individual components. Then retry or prepare the fictional demonstration database below. Existing data and passwords are preserved. No terminal is required.";
        }

        private async System.Threading.Tasks.Task OpenDatabase(bool prepare)
        {
            btnPrepare.Enabled = btnRetry.Enabled = false;
            UseWaitCursor = true;
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    if (prepare)
                    {
                        FuturePath.Data.DatabaseSetup.PrepareDemo(app.Store.DatabaseName);
                    }
                    else
                    {
                        app.Store.Initialize();
                    }
                });
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
            finally
            {
                btnPrepare.Enabled = btnRetry.Enabled = true;
                UseWaitCursor = false;
            }
        }

        private async void btnPrepare_Click(object? sender, EventArgs e)
        {
            await OpenDatabase(true);
        }

        private async void btnRetry_Click(object? sender, EventArgs e)
        {
            await OpenDatabase(false);
        }
    }
}
