#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmEditFundingFaculty
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lblcboFundingId = null!;
        private ComboBox cboFundingId = null!;
        private Label lblcboFacultyId = null!;
        private ComboBox cboFacultyId = null!;
        private CheckBox chkActive = null!;
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
            lblcboFundingId = new Label();
            lblcboFundingId.Name = "lblcboFundingId";
            cboFundingId = new ComboBox();
            cboFundingId.Name = "cboFundingId";
            lblcboFacultyId = new Label();
            lblcboFacultyId.Name = "lblcboFacultyId";
            cboFacultyId = new ComboBox();
            cboFacultyId.Name = "cboFacultyId";
            chkActive = new CheckBox();
            chkActive.Name = "chkActive";
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
            lblHeading.Text = "Maintain FundingFaculty";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            lblcboFundingId.Text = "Funding opportunity";
            lblcboFundingId.AutoSize = true;
            lblcboFundingId.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblcboFundingId, 0, 2);
            cboFundingId.Name = "cboFundingId";
            cboFundingId.AccessibleName = "Funding opportunity";
            cboFundingId.Dock = DockStyle.Fill;
            cboFundingId.Margin = new Padding(5, 3, 5, 8);
            cboFundingId.DropDownStyle = ComboBoxStyle.DropDownList;
            tblDialog.Controls.Add(cboFundingId, 0, 3);
            lblcboFacultyId.Text = "Faculty";
            lblcboFacultyId.AutoSize = true;
            lblcboFacultyId.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblcboFacultyId, 0, 4);
            cboFacultyId.Name = "cboFacultyId";
            cboFacultyId.AccessibleName = "Faculty";
            cboFacultyId.Dock = DockStyle.Fill;
            cboFacultyId.Margin = new Padding(5, 3, 5, 8);
            cboFacultyId.DropDownStyle = ComboBoxStyle.DropDownList;
            tblDialog.Controls.Add(cboFacultyId, 0, 5);
            chkActive.Name = "chkActive";
            chkActive.AccessibleName = "chkActive";
            chkActive.Dock = DockStyle.Fill;
            chkActive.Margin = new Padding(5, 3, 5, 8);
            chkActive.Text = "Active";
            chkActive.AutoSize = true;
            tblDialog.Controls.Add(chkActive, 0, 6);
            btnSave.Name = "btnSave";
            btnSave.AccessibleName = "btnSave";
            btnSave.Dock = DockStyle.Fill;
            btnSave.Margin = new Padding(5, 3, 5, 8);
            btnSave.AutoSize = true;
            btnSave.MinimumSize = new Size(0, 40);
            btnSave.Text = "Save changes";
            btnSave.Click += btnSave_Click;
            tblDialog.Controls.Add(btnSave, 0, 7);
            tblDialog.RowCount = 8;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Maintain FundingFaculty";
            Name = "frmEditFundingFaculty";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
