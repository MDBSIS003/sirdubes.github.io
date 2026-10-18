#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmReports
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lbldtpFrom = null!;
        private DateTimePicker dtpFrom = null!;
        private Label lbldtpTo = null!;
        private DateTimePicker dtpTo = null!;
        private Label lbltxtSearch = null!;
        private TextBox txtSearch = null!;
        private Button btnActivity = null!;
        private Button btnExceptions = null!;
        private Label lblcboFaculty = null!;
        private ComboBox cboFaculty = null!;
        private Label lblcboDegree = null!;
        private ComboBox cboDegree = null!;
        private Label lblcboOutcome = null!;
        private ComboBox cboOutcome = null!;
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
            lbldtpFrom = new Label();
            lbldtpFrom.Name = "lbldtpFrom";
            dtpFrom = new DateTimePicker();
            dtpFrom.Name = "dtpFrom";
            lbldtpTo = new Label();
            lbldtpTo.Name = "lbldtpTo";
            dtpTo = new DateTimePicker();
            dtpTo.Name = "dtpTo";
            lbltxtSearch = new Label();
            lbltxtSearch.Name = "lbltxtSearch";
            txtSearch = new TextBox();
            txtSearch.Name = "txtSearch";
            btnActivity = new Button();
            btnActivity.Name = "btnActivity";
            btnExceptions = new Button();
            btnExceptions.Name = "btnExceptions";
            lblcboFaculty = new Label();
            cboFaculty = new ComboBox();
            lblcboDegree = new Label();
            cboDegree = new ComboBox();
            lblcboOutcome = new Label();
            cboOutcome = new ComboBox();
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
            lblHeading.Text = "Advising reports";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            lbldtpFrom.Text = "From (UTC)";
            lbldtpFrom.AutoSize = true;
            lbldtpFrom.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbldtpFrom, 0, 2);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.AccessibleName = "From (UTC)";
            dtpFrom.Dock = DockStyle.Fill;
            dtpFrom.Margin = new Padding(5, 3, 5, 8);
            tblDialog.Controls.Add(dtpFrom, 0, 3);
            lbldtpTo.Text = "To (UTC)";
            lbldtpTo.AutoSize = true;
            lbldtpTo.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbldtpTo, 0, 4);
            dtpTo.Name = "dtpTo";
            dtpTo.AccessibleName = "To (UTC)";
            dtpTo.Dock = DockStyle.Fill;
            dtpTo.Margin = new Padding(5, 3, 5, 8);
            tblDialog.Controls.Add(dtpTo, 0, 5);
            lbltxtSearch.Text = "Search learner / degree";
            lbltxtSearch.AutoSize = true;
            lbltxtSearch.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtSearch, 0, 6);
            txtSearch.Name = "txtSearch";
            txtSearch.AccessibleName = "Search learner / degree";
            txtSearch.Dock = DockStyle.Fill;
            txtSearch.Margin = new Padding(5, 3, 5, 8);
            tblDialog.Controls.Add(txtSearch, 0, 7);
            btnActivity.Name = "btnActivity";
            btnActivity.AccessibleName = "btnActivity";
            btnActivity.Dock = DockStyle.Fill;
            btnActivity.Margin = new Padding(5, 3, 5, 8);
            btnActivity.AutoSize = true;
            btnActivity.MinimumSize = new Size(0, 40);
            btnActivity.Text = "Degree Interest and Eligibility Report";
            btnActivity.Click += btnActivity_Click;
            tblDialog.Controls.Add(btnActivity, 0, 14);
            btnExceptions.Name = "btnExceptions";
            btnExceptions.AccessibleName = "btnExceptions";
            btnExceptions.Dock = DockStyle.Fill;
            btnExceptions.Margin = new Padding(5, 3, 5, 8);
            btnExceptions.AutoSize = true;
            btnExceptions.MinimumSize = new Size(0, 40);
            btnExceptions.Text = "Not Currently Eligible / More Information Needed Report";
            btnExceptions.Click += btnExceptions_Click;
            tblDialog.Controls.Add(btnExceptions, 0, 15);
            tblDialog.RowCount = 16;
            lblcboFaculty.Text = "Faculty (all includes inactive historical records)";
            lblcboFaculty.AutoSize = true;
            lblcboFaculty.Margin = new Padding(5, 10, 5, 3);
            cboFaculty.Name = "cboFaculty";
            cboFaculty.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFaculty.Dock = DockStyle.Fill;
            cboFaculty.Margin = new Padding(5, 3, 5, 8);
            tblDialog.Controls.Add(lblcboFaculty, 0, 8);
            tblDialog.Controls.Add(cboFaculty, 0, 9);
            lblcboDegree.Text = "Degree";
            lblcboDegree.AutoSize = true;
            lblcboDegree.Margin = new Padding(5, 10, 5, 3);
            cboDegree.Name = "cboDegree";
            cboDegree.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDegree.Dock = DockStyle.Fill;
            cboDegree.Margin = new Padding(5, 3, 5, 8);
            tblDialog.Controls.Add(lblcboDegree, 0, 10);
            tblDialog.Controls.Add(cboDegree, 0, 11);
            lblcboOutcome.Text = "Eligibility outcome";
            lblcboOutcome.AutoSize = true;
            lblcboOutcome.Margin = new Padding(5, 10, 5, 3);
            cboOutcome.Name = "cboOutcome";
            cboOutcome.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOutcome.Dock = DockStyle.Fill;
            cboOutcome.Margin = new Padding(5, 3, 5, 8);
            tblDialog.Controls.Add(lblcboOutcome, 0, 12);
            tblDialog.Controls.Add(cboOutcome, 0, 13);
            cboFaculty.SelectedIndexChanged += cboFaculty_SelectedIndexChanged;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Advising reports";
            Name = "frmReports";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
