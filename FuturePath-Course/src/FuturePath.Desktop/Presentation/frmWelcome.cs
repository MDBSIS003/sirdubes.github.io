using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmWelcome : frmStaffForm
    {
        public frmWelcome()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        private frmMain main = null!;
        public frmWelcome(AppController app, frmMain main) : this()
        {
            lblDisclaimer.Text = RecommendationEngine.Disclaimer + Environment.NewLine + lblDisclaimer.Text;
            this.app = app;
            this.main = main;
            FormBorderStyle = FormBorderStyle.None;
            ControlBox = false;
            btnLogin.Text = app.Access.IsStaff ? "View saved learner sessions" : "Staff sign in";
            btnReports.Visible = app.Access.IsStaff;
            btnAdministration.Visible = app.Access.Role == UserRole.Administrator;
            if (app.Access.IsStaff)
            {
                lblHeading.Text = "Staff workspace";
                lblIntroduction.Text = "Signed in as " + app.Access.DisplayName + " (" + app.Access.Role + ")." + Environment.NewLine +
                    "View saved learner information, create follow-up assessments and generate reports.";
            }
        }

        private void btnStart_Click(object? sender, EventArgs e)
        {
            main.StartAdvisory();
        }

        private void btnReports_Click(object? sender, EventArgs e)
        {
            using (frmReports form = new frmReports(app))
            {
                form.ShowDialog(this);
            }
        }

        private void btnAdministration_Click(object? sender, EventArgs e)
        {
            using (frmAdminMaintenance form = new frmAdminMaintenance(app))
            {
                form.ShowDialog(this);
            }
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            if (app.Access.IsStaff)
            {
                using (frmLearnerSessions form = new frmLearnerSessions(app, main))
                {
                    form.ShowDialog(this);
                }
            }
            else
            {
                using (frmLogin form = new frmLogin(app))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        main.ShowOverview();
                    }
                }
            }
        }
    }
}
