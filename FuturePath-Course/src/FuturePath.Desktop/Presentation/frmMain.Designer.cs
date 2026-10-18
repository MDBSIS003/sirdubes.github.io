#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmMain
    {
        private System.ComponentModel.IContainer? components;
        private MenuStrip mnuStaff = null!;
        private ToolStripMenuItem mnuHome = null!;
        private ToolStripMenuItem mnuSessions = null!;
        private ToolStripMenuItem mnuReports = null!;
        private ToolStripMenuItem mnuAdmin = null!;
        private ToolStripMenuItem mnuPassword = null!;
        private ToolStripMenuItem mnuLogout = null!;
        private StatusStrip status = null!;
        private ToolStripStatusLabel lblStatus = null!;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            mnuStaff = new MenuStrip();
            mnuStaff.Name = "mnuStaff";
            mnuHome = new ToolStripMenuItem();
            mnuHome.Name = "mnuHome";
            mnuSessions = new ToolStripMenuItem();
            mnuSessions.Name = "mnuSessions";
            mnuReports = new ToolStripMenuItem();
            mnuReports.Name = "mnuReports";
            mnuAdmin = new ToolStripMenuItem();
            mnuAdmin.Name = "mnuAdmin";
            mnuPassword = new ToolStripMenuItem();
            mnuPassword.Name = "mnuPassword";
            mnuLogout = new ToolStripMenuItem();
            mnuLogout.Name = "mnuLogout";
            status = new StatusStrip();
            status.Name = "status";
            lblStatus = new ToolStripStatusLabel();
            lblStatus.Name = "lblStatus";
            SuspendLayout();
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            ClientSize = new Size(1180, 760);
            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;
            Text = "Future Path | Student Advising";
            MainMenuStrip = mnuStaff;
            mnuHome.Text = "Overview";
            mnuSessions.Text = "Learners and sessions";
            mnuReports.Text = "Reports";
            mnuAdmin.Text = "Administration";
            mnuPassword.Text = "Change password";
            mnuLogout.Text = "Sign out";
            mnuHome.Click += mnuHome_Click;
            mnuSessions.Click += mnuSessions_Click;
            mnuReports.Click += mnuReports_Click;
            mnuAdmin.Click += mnuAdmin_Click;
            mnuPassword.Click += mnuPassword_Click;
            mnuLogout.Click += mnuLogout_Click;
            mnuStaff.Items.AddRange(new ToolStripItem[] {
                mnuHome,
                mnuSessions,
                mnuReports,
                mnuAdmin,
                mnuPassword,
                mnuLogout
            });
            mnuStaff.Visible = false;
            status.Items.Add(lblStatus);
            lblStatus.Text = "Works offline | Sample information - verify before applying";
            Controls.Add(mnuStaff);
            Controls.Add(status);
            Shown += frmMain_Shown;
            FormClosing += frmMain_FormClosing;
            Name = "frmMain";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
