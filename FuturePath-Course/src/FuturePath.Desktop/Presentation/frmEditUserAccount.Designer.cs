#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmEditUserAccount
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lbltxtUsername = null!;
        private TextBox txtUsername = null!;
        private Label lblcboRole = null!;
        private ComboBox cboRole = null!;
        private CheckBox chkActive = null!;
        private Label lbltxtPassword = null!;
        private TextBox txtPassword = null!;
        private Button btnSave = null!;
        private Label lblConfirm = null!;
        private TextBox txtConfirm = null!;
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
            lblcboRole = new Label();
            lblcboRole.Name = "lblcboRole";
            cboRole = new ComboBox();
            cboRole.Name = "cboRole";
            chkActive = new CheckBox();
            chkActive.Name = "chkActive";
            lbltxtPassword = new Label();
            lbltxtPassword.Name = "lbltxtPassword";
            txtPassword = new TextBox();
            txtPassword.Name = "txtPassword";
            btnSave = new Button();
            btnSave.Name = "btnSave";
            lblConfirm = new Label();
            lblConfirm.Name = "lblConfirm";
            txtConfirm = new TextBox();
            txtConfirm.Name = "txtConfirm";
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
            lblHeading.Text = "Maintain staff account";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            lbltxtUsername.Text = "Username";
            lbltxtUsername.AutoSize = true;
            lbltxtUsername.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtUsername, 0, 2);
            txtUsername.Name = "txtUsername";
            txtUsername.AccessibleName = "Username";
            txtUsername.Dock = DockStyle.Fill;
            txtUsername.Margin = new Padding(5, 3, 5, 8);
            txtUsername.MaxLength = 60;
            tblDialog.Controls.Add(txtUsername, 0, 3);
            lblcboRole.Text = "Role";
            lblcboRole.AutoSize = true;
            lblcboRole.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblcboRole, 0, 4);
            cboRole.Name = "cboRole";
            cboRole.AccessibleName = "Role";
            cboRole.Dock = DockStyle.Fill;
            cboRole.Margin = new Padding(5, 3, 5, 8);
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            tblDialog.Controls.Add(cboRole, 0, 5);
            chkActive.Name = "chkActive";
            chkActive.AccessibleName = "chkActive";
            chkActive.Dock = DockStyle.Fill;
            chkActive.Margin = new Padding(5, 3, 5, 8);
            chkActive.Text = "Active";
            chkActive.AutoSize = true;
            tblDialog.Controls.Add(chkActive, 0, 6);
            lbltxtPassword.Text = "New password (leave blank to retain existing password)";
            lbltxtPassword.AutoSize = true;
            lbltxtPassword.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtPassword, 0, 7);
            txtPassword.Name = "txtPassword";
            txtPassword.AccessibleName = "New password (leave blank to retain existing password)";
            txtPassword.Dock = DockStyle.Fill;
            txtPassword.Margin = new Padding(5, 3, 5, 8);
            txtPassword.UseSystemPasswordChar = true;
            txtPassword.MaxLength = 128;
            tblDialog.Controls.Add(txtPassword, 0, 8);
            btnSave.Name = "btnSave";
            btnSave.AccessibleName = "btnSave";
            btnSave.Dock = DockStyle.Fill;
            btnSave.Margin = new Padding(5, 3, 5, 8);
            btnSave.AutoSize = true;
            btnSave.MinimumSize = new Size(0, 40);
            btnSave.Text = "Save staff account";
            btnSave.Click += btnSave_Click;
            lblConfirm.Text = "Confirm new password";
            lblConfirm.AutoSize = true;
            lblConfirm.Margin = new Padding(5, 10, 5, 3);
            txtConfirm.Dock = DockStyle.Fill;
            txtConfirm.UseSystemPasswordChar = true;
            txtConfirm.MaxLength = 128;
            txtConfirm.AccessibleName = "Confirm new password";
            txtConfirm.Margin = new Padding(5, 3, 5, 8);
            tblDialog.Controls.Add(lblConfirm, 0, 9);
            tblDialog.Controls.Add(txtConfirm, 0, 10);
            tblDialog.Controls.Add(btnSave, 0, 11);
            tblDialog.RowCount = 12;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Maintain staff account";
            Name = "frmEditUserAccount";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
