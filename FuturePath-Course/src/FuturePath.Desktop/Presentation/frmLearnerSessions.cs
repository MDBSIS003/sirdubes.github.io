using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmLearnerSessions : frmStaffForm
    {
        public frmLearnerSessions()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        private frmMain main = null!;
        public frmLearnerSessions(AppController app, frmMain main) : this()
        {
            this.app = app;
            this.main = main;
            AcceptButton = btnSearch;
            app.Access.RequireStaff();
            dtpFrom.Value = dtpFrom.MinDate;
            dtpTo.Value = DateTime.UtcNow.Date;
            btnSearch_Click(this, EventArgs.Empty);
        }

        private void btnSearch_Click(object? sender, EventArgs e)
        {
            try
            {
                List<SessionSummary> sessions = app.Advising.Search(DateOnly.FromDateTime(dtpFrom.Value), DateOnly.FromDateTime(dtpTo.Value), txtSearch.Text);
                lstSessions.Items.Clear();
                foreach (SessionSummary session in sessions)
                {
                    ListViewItem item = new ListViewItem("FP-" + session.Id);
                    item.SubItems.Add(session.Learner);
                    item.SubItems.Add(session.Grade.ToString());
                    item.SubItems.Add(session.Programme);
                    item.SubItems.Add(session.Outcome);
                    item.SubItems.Add(session.CreatedUtc.ToLocalTime().ToString("yyyy-MM-dd"));
                    item.Tag = session.Id;
                    lstSessions.Items.Add(item);
                }

                lblError.Text = sessions.Count + " sessions found.";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private int SelectedId()
        {
            if (lstSessions.SelectedItems.Count == 0)
            {
                throw new ValidationException("Select a saved session.");
            }

            return (int)lstSessions.SelectedItems[0].Tag!;
        }

        private void btnSummary_Click(object? sender, EventArgs e)
        {
            try
            {
                using (frmReportPreview form = new frmReportPreview(app, app.Reports.Summary(SelectedId())))
                {
                    form.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void btnFollowUp_Click(object? sender, EventArgs e)
        {
            try
            {
                AdvisorySession session = app.Advising.Load(SelectedId());
                Close();
                main.StartAdvisory(session.Input);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
