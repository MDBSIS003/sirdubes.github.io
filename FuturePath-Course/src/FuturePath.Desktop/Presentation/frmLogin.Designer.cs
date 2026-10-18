#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmLogin
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lbltxtUsername = null!;
        private TextBox txtUsername = null!;
        private Label lbltxtPassword = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
        private Button btnCreateStaff = null!;
        private Label lblAccountHelp = null!;
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
            lbltxtUsername = new Label();
            lbltxtUsername.Name = "lbltxtUsername";
            txtUsername = new TextBox();
            txtUsername.Name = "txtUsername";
            lbltxtPassword = new Label();
            lbltxtPassword.Name = "lbltxtPassword";
            txtPassword = new TextBox();
            txtPassword.Name = "txtPassword";
            btnLogin = new Button();
            btnLogin.Name = "btnLogin";
            btnCreateStaff = new Button();
            btnCreateStaff.Name = "btnCreateStaff";
            lblAccountHelp = new Label();
            lblAccountHelp.Name = "lblAccountHelp";
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
            lblHeading.Text = "Staff sign in";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            lbltxtUsername.Text = "Username or email";
            lbltxtUsername.AutoSize = true;
            lbltxtUsername.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtUsername, 0, 2);
            txtUsername.Name = "txtUsername";
            txtUsername.AccessibleName = "Username or email";
            txtUsername.Dock = DockStyle.Fill;
            txtUsername.Margin = new Padding(5, 3, 5, 8);
            txtUsername.MaxLength = 150;
            tblDialog.Controls.Add(txtUsername, 0, 3);
            lbltxtPassword.Text = "Password";
            lbltxtPassword.AutoSize = true;
            lbltxtPassword.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtPassword, 0, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.AccessibleName = "Password";
            txtPassword.Dock = DockStyle.Fill;
            txtPassword.Margin = new Padding(5, 3, 5, 8);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.MaxLength = 128;
            tblDialog.Controls.Add(txtPassword, 0, 5);
            btnLogin.Name = "btnLogin";
            btnLogin.AccessibleName = "btnLogin";
            btnLogin.Dock = DockStyle.Fill;
            btnLogin.Margin = new Padding(5, 3, 5, 8);
            btnLogin.AutoSize = true;
            btnLogin.MinimumSize = new Size(0, 40);
            btnLogin.Text = "Sign in";
            btnLogin.Click += btnLogin_Click;
            tblDialog.Controls.Add(btnLogin, 0, 6);
            btnCreateStaff.Text = "Create tester staff account";
            btnCreateStaff.AutoSize = true;
            btnCreateStaff.Dock = DockStyle.Fill;
            btnCreateStaff.MinimumSize = new Size(0, 40);
            btnCreateStaff.Margin = new Padding(5, 3, 5, 8);
            btnCreateStaff.Click += btnCreateStaff_Click;
            lblAccountHelp.AutoSize = true;
            lblAccountHelp.Dock = DockStyle.Fill;
            lblAccountHelp.Margin = new Padding(5, 8, 5, 8);
            lblAccountHelp.Text = "For offline assessment testing, create your own staff account with the simulated email-code option. Existing staff can sign in above.";
            tblDialog.Controls.Add(btnCreateStaff, 0, 7);
            tblDialog.Controls.Add(lblAccountHelp, 0, 8);
            tblDialog.RowCount = 9;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Staff sign in";
            Name = "frmLogin";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
