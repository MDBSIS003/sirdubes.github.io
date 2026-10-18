using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmReportPreview : frmStaffForm
    {
        public frmReportPreview()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        private ReportDocument report = null!;
        public frmReportPreview(AppController app, ReportDocument report) : this()
        {
            this.app = app;
            this.report = report;
            lblHeading.Text = report.Title;
            txtReport.Text = report.Content;
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            ReportFiles.Save(this, app.Reports, report);
        }

        private void btnPrint_Click(object? sender, EventArgs e)
        {
            ReportFiles.Print(this, report);
        }
    }
}
