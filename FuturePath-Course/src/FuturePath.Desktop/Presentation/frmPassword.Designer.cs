#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmPassword
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lbltxtCurrent = null!;
        private TextBox txtCurrent = null!;
        private Label lbltxtNew = null!;
        private TextBox txtNew = null!;
        private Label lbltxtConfirm = null!;
        private TextBox txtConfirm = null!;
        private Button btnSave = null!;
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
            lbltxtCurrent = new Label();
            lbltxtCurrent.Name = "lbltxtCurrent";
            txtCurrent = new TextBox();
            txtCurrent.Name = "txtCurrent";
            lbltxtNew = new Label();
            lbltxtNew.Name = "lbltxtNew";
            txtNew = new TextBox();
            txtNew.Name = "txtNew";
            lbltxtConfirm = new Label();
            lbltxtConfirm.Name = "lbltxtConfirm";
            txtConfirm = new TextBox();
            txtConfirm.Name = "txtConfirm";
            btnSave = new Button();
            btnSave.Name = "btnSave";
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
            lblHeading.Text = "Change staff password";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            lbltxtCurrent.Text = "Current password";
            lbltxtCurrent.AutoSize = true;
            lbltxtCurrent.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtCurrent, 0, 2);
            txtCurrent.Name = "txtCurrent";
            txtCurrent.AccessibleName = "Current password";
            txtCurrent.Dock = DockStyle.Fill;
            txtCurrent.Margin = new Padding(5, 3, 5, 8);
            txtCurrent.UseSystemPasswordChar = true;
            txtCurrent.MaxLength = 128;
            tblDialog.Controls.Add(txtCurrent, 0, 3);
            lbltxtNew.Text = "New password (12-128 characters, letter and number)";
            lbltxtNew.AutoSize = true;
            lbltxtNew.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtNew, 0, 4);
            txtNew.Name = "txtNew";
            txtNew.AccessibleName = "New password (12-128 characters, letter and number)";
            txtNew.Dock = DockStyle.Fill;
            txtNew.Margin = new Padding(5, 3, 5, 8);
            txtNew.UseSystemPasswordChar = true;
            txtNew.MaxLength = 128;
            tblDialog.Controls.Add(txtNew, 0, 5);
            lbltxtConfirm.Text = "Confirm new password";
            lbltxtConfirm.AutoSize = true;
            lbltxtConfirm.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtConfirm, 0, 6);
            txtConfirm.Name = "txtConfirm";
            txtConfirm.AccessibleName = "Confirm new password";
            txtConfirm.Dock = DockStyle.Fill;
            txtConfirm.Margin = new Padding(5, 3, 5, 8);
            txtConfirm.UseSystemPasswordChar = true;
            txtConfirm.MaxLength = 128;
            tblDialog.Controls.Add(txtConfirm, 0, 7);
            btnSave.Name = "btnSave";
            btnSave.AccessibleName = "btnSave";
            btnSave.Dock = DockStyle.Fill;
            btnSave.Margin = new Padding(5, 3, 5, 8);
            btnSave.AutoSize = true;
            btnSave.MinimumSize = new Size(0, 40);
            btnSave.Text = "Save password";
            btnSave.Click += btnSave_Click;
            tblDialog.Controls.Add(btnSave, 0, 8);
            tblDialog.RowCount = 9;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Change staff password";
            Name = "frmPassword";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
