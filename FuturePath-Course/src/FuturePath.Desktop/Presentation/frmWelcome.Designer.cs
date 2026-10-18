#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmWelcome
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lblIntroduction = null!;
        private Button btnStart = null!;
        private Button btnLogin = null!;
        private Label lblDisclaimer = null!;
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
            tblDialog = new TableLayoutPanel();
            tblDialog.Name = "tblDialog";
            pnlScroll = new Panel();
            pnlScroll.Name = "pnlScroll";
            lblHeading = new Label();
            lblHeading.Name = "lblHeading";
            lblError = new Label();
            lblError.Name = "lblError";
            lblIntroduction = new Label();
            lblIntroduction.Name = "lblIntroduction";
            btnStart = new Button();
            btnStart.Name = "btnStart";
            btnLogin = new Button();
            btnLogin.Name = "btnLogin";
            lblDisclaimer = new Label();
            lblDisclaimer.Name = "lblDisclaimer";
            SuspendLayout();
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            ClientSize = new Size(950, 700);
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            pnlScroll.Dock = DockStyle.Fill;
            pnlScroll.AutoScroll = true;
            tblDialog.Dock = DockStyle.Top;
            tblDialog.AutoSize = true;
            tblDialog.ColumnCount = 1;
            tblDialog.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblDialog.Padding = new Padding(16);
            lblHeading.AutoSize = true;
            lblHeading.Dock = DockStyle.Fill;
            lblHeading.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeading.Text = "Plan your future";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            lblIntroduction.Name = "lblIntroduction";
            lblIntroduction.AccessibleName = "lblIntroduction";
            lblIntroduction.Dock = DockStyle.Fill;
            lblIntroduction.Margin = new Padding(5, 3, 5, 8);
            lblIntroduction.Text = "1. Capture your profile and marks.\r\n2. Explore interests, programmes and alternatives.\r\n3. Choose funding guidance and save your advisory summary.\r\n\r\nPersonal name and contact details are optional. School, province and grade are required.";
            lblIntroduction.AutoSize = true;
            tblDialog.Controls.Add(lblIntroduction, 0, 2);
            btnStart.Name = "btnStart";
            btnStart.AccessibleName = "btnStart";
            btnStart.Dock = DockStyle.Fill;
            btnStart.Margin = new Padding(5, 3, 5, 8);
            btnStart.AutoSize = true;
            btnStart.MinimumSize = new Size(0, 40);
            btnStart.Text = "Start an advisory session";
            btnStart.Click += btnStart_Click;
            tblDialog.Controls.Add(btnStart, 0, 3);
            btnLogin.Name = "btnLogin";
            btnLogin.AccessibleName = "btnLogin";
            btnLogin.Dock = DockStyle.Fill;
            btnLogin.Margin = new Padding(5, 3, 5, 8);
            btnLogin.AutoSize = true;
            btnLogin.MinimumSize = new Size(0, 40);
            btnLogin.Text = "Staff sign in";
            btnLogin.Click += btnLogin_Click;
            tblDialog.Controls.Add(btnLogin, 0, 4);
            lblDisclaimer.Name = "lblDisclaimer";
            lblDisclaimer.AccessibleName = "lblDisclaimer";
            lblDisclaimer.Dock = DockStyle.Fill;
            lblDisclaimer.Margin = new Padding(5, 3, 5, 8);
            lblDisclaimer.Text = "\r\n\r\nEnd your session before another learner uses this computer. Only signed-in staff can retrieve other saved sessions.";
            lblDisclaimer.AutoSize = true;
            tblDialog.Controls.Add(lblDisclaimer, 0, 5);
            tblDialog.RowCount = 6;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Plan your future";
            Name = "frmWelcome";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
