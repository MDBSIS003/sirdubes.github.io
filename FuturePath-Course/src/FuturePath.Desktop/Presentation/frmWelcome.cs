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
            btnLogin.Text = app.Access.IsStaff ? "View saved sessions" : "Staff sign in";
        }

        private void btnStart_Click(object? sender, EventArgs e)
        {
            main.StartAdvisory();
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
