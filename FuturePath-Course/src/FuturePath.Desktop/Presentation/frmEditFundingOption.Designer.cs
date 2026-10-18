#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmEditFundingOption
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lbltxtName = null!;
        private TextBox txtName = null!;
        private Label lblcboType = null!;
        private ComboBox cboType = null!;
        private Label lbltxtEligibility = null!;
        private TextBox txtEligibility = null!;
        private Label lbltxtDocuments = null!;
        private TextBox txtDocuments = null!;
        private Label lbldtpClosingDate = null!;
        private DateTimePicker dtpClosingDate = null!;
        private Label lbltxtContact = null!;
        private TextBox txtContact = null!;
        private CheckBox chkSample = null!;
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
            lbltxtName = new Label();
            lbltxtName.Name = "lbltxtName";
            txtName = new TextBox();
            txtName.Name = "txtName";
            lblcboType = new Label();
            lblcboType.Name = "lblcboType";
            cboType = new ComboBox();
            cboType.Name = "cboType";
            lbltxtEligibility = new Label();
            lbltxtEligibility.Name = "lbltxtEligibility";
            txtEligibility = new TextBox();
            txtEligibility.Name = "txtEligibility";
            lbltxtDocuments = new Label();
            lbltxtDocuments.Name = "lbltxtDocuments";
            txtDocuments = new TextBox();
            txtDocuments.Name = "txtDocuments";
            lbldtpClosingDate = new Label();
            lbldtpClosingDate.Name = "lbldtpClosingDate";
            dtpClosingDate = new DateTimePicker();
            dtpClosingDate.Name = "dtpClosingDate";
            lbltxtContact = new Label();
            lbltxtContact.Name = "lbltxtContact";
            txtContact = new TextBox();
            txtContact.Name = "txtContact";
            chkSample = new CheckBox();
            chkSample.Name = "chkSample";
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
            lblHeading.Text = "Maintain FundingOption";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            lbltxtName.Text = "Funding name";
            lbltxtName.AutoSize = true;
            lbltxtName.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtName, 0, 2);
            txtName.Name = "txtName";
            txtName.AccessibleName = "Funding name";
            txtName.Dock = DockStyle.Fill;
            txtName.Margin = new Padding(5, 3, 5, 8);
            txtName.MaxLength = 150;
            tblDialog.Controls.Add(txtName, 0, 3);
            lblcboType.Text = "Funding type";
            lblcboType.AutoSize = true;
            lblcboType.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblcboType, 0, 4);
            cboType.Name = "cboType";
            cboType.AccessibleName = "Funding type";
            cboType.Dock = DockStyle.Fill;
            cboType.Margin = new Padding(5, 3, 5, 8);
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            tblDialog.Controls.Add(cboType, 0, 5);
            lbltxtEligibility.Text = "Eligibility / merit / financial need";
            lbltxtEligibility.AutoSize = true;
            lbltxtEligibility.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtEligibility, 0, 6);
            txtEligibility.Name = "txtEligibility";
            txtEligibility.AccessibleName = "Eligibility / merit / financial need";
            txtEligibility.Dock = DockStyle.Fill;
            txtEligibility.Margin = new Padding(5, 3, 5, 8);
            txtEligibility.MaxLength = 2000;
            txtEligibility.Multiline = true;
            txtEligibility.Height = 85;
            txtEligibility.ScrollBars = ScrollBars.Vertical;
            tblDialog.Controls.Add(txtEligibility, 0, 7);
            lbltxtDocuments.Text = "Required documents";
            lbltxtDocuments.AutoSize = true;
            lbltxtDocuments.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtDocuments, 0, 8);
            txtDocuments.Name = "txtDocuments";
            txtDocuments.AccessibleName = "Required documents";
            txtDocuments.Dock = DockStyle.Fill;
            txtDocuments.Margin = new Padding(5, 3, 5, 8);
            txtDocuments.MaxLength = 2000;
            txtDocuments.Multiline = true;
            txtDocuments.Height = 85;
            txtDocuments.ScrollBars = ScrollBars.Vertical;
            tblDialog.Controls.Add(txtDocuments, 0, 9);
            lbldtpClosingDate.Text = "Closing date";
            lbldtpClosingDate.AutoSize = true;
            lbldtpClosingDate.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbldtpClosingDate, 0, 10);
            dtpClosingDate.Name = "dtpClosingDate";
            dtpClosingDate.AccessibleName = "Closing date";
            dtpClosingDate.Dock = DockStyle.Fill;
            dtpClosingDate.Margin = new Padding(5, 3, 5, 8);
            tblDialog.Controls.Add(dtpClosingDate, 0, 11);
            lbltxtContact.Text = "Contact / application information";
            lbltxtContact.AutoSize = true;
            lbltxtContact.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtContact, 0, 12);
            txtContact.Name = "txtContact";
            txtContact.AccessibleName = "Contact / application information";
            txtContact.Dock = DockStyle.Fill;
            txtContact.Margin = new Padding(5, 3, 5, 8);
            txtContact.MaxLength = 2000;
            txtContact.Multiline = true;
            txtContact.Height = 85;
            txtContact.ScrollBars = ScrollBars.Vertical;
            tblDialog.Controls.Add(txtContact, 0, 13);
            chkSample.Name = "chkSample";
            chkSample.AccessibleName = "chkSample";
            chkSample.Dock = DockStyle.Fill;
            chkSample.Margin = new Padding(5, 3, 5, 8);
            chkSample.Text = "Sample / verify information";
            chkSample.AutoSize = true;
            tblDialog.Controls.Add(chkSample, 0, 14);
            chkActive.Name = "chkActive";
            chkActive.AccessibleName = "chkActive";
            chkActive.Dock = DockStyle.Fill;
            chkActive.Margin = new Padding(5, 3, 5, 8);
            chkActive.Text = "Active";
            chkActive.AutoSize = true;
            tblDialog.Controls.Add(chkActive, 0, 15);
            btnSave.Name = "btnSave";
            btnSave.AccessibleName = "btnSave";
            btnSave.Dock = DockStyle.Fill;
            btnSave.Margin = new Padding(5, 3, 5, 8);
            btnSave.AutoSize = true;
            btnSave.MinimumSize = new Size(0, 40);
            btnSave.Text = "Save changes";
            btnSave.Click += btnSave_Click;
            tblDialog.Controls.Add(btnSave, 0, 16);
            tblDialog.RowCount = 17;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Maintain FundingOption";
            Name = "frmEditFundingOption";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
