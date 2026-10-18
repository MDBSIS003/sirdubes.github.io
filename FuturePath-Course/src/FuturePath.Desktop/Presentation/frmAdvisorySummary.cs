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
            WrapDisplayLabel(lblSummary);
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
            ShowMessage("Assessment saved. Your reference is FP-" + main.SavedSession.Id + ". You may save a copy or select Finish.");
            btnExport.Enabled = btnPrint.Enabled = false;
            btnRetry.Visible = false;
            report = null;
            try
            {
                report = app.Reports.Summary(main.SavedSession.Id, detailed: false);
                lblSummary.Text = report.Content;
                btnExport.Enabled = btnPrint.Enabled = true;
            }
            catch (Exception ex)
            {
                lblSummary.Text = "Your assessment is saved. The summary could not be loaded. Choose Retry summary, or give your reference to a career advisor." +
                    Environment.NewLine + Environment.NewLine + ex.Message;
                btnRetry.Visible = true;
            }
        }

        private void btnRetry_Click(object? sender, EventArgs e)
        {
            RefreshStep();
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
