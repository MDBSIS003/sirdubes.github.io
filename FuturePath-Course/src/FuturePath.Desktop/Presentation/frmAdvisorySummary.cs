using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmAdvisorySummary : frmAdvisoryStep
    {
        #region Constructors
        public frmAdvisorySummary()
        {
            InitializeComponent();
        }

        public frmAdvisorySummary(AppController app, frmMain main) : this()
        {
            this.app = app;
            this.main = main;
        }

        #endregion
        private ReportDocument? report;
        public override void RefreshStep()
        {
            if (main.SavedSession == null)
            {
                return;
            }

            lblMessage.ForeColor = Color.DarkGreen;
            report = app.Reports.Summary(main.SavedSession.Id);
            txtSummary.Text = report.Content;
            ShowMessage("Saved as session #" + main.SavedSession.Id + ". Export or print your summary before finishing.");
        }

        private void btnExport_Click(object? sender, EventArgs e)
        {
            if (report != null)
            {
                ReportFiles.Save(this, app.Reports, report);
            }
        }

        private void btnPrint_Click(object? sender, EventArgs e)
        {
            if (report != null)
            {
                ReportFiles.Print(this, report);
            }
        }
    }
}
