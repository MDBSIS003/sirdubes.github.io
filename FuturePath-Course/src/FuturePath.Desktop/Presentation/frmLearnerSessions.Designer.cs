#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmLearnerSessions
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lbltxtSearch = null!;
        private TextBox txtSearch = null!;
        private Label lbldtpFrom = null!;
        private DateTimePicker dtpFrom = null!;
        private Label lbldtpTo = null!;
        private DateTimePicker dtpTo = null!;
        private Button btnSearch = null!;
        private Label lbllstSessions = null!;
        private ListView lstSessions = null!;
        private Button btnSummary = null!;
        private Button btnFollowUp = null!;
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
            lbltxtSearch = new Label();
            lbltxtSearch.Name = "lbltxtSearch";
            txtSearch = new TextBox();
            txtSearch.Name = "txtSearch";
            lbldtpFrom = new Label();
            lbldtpFrom.Name = "lbldtpFrom";
            dtpFrom = new DateTimePicker();
            dtpFrom.Name = "dtpFrom";
            lbldtpTo = new Label();
            lbldtpTo.Name = "lbldtpTo";
            dtpTo = new DateTimePicker();
            dtpTo.Name = "dtpTo";
            btnSearch = new Button();
            btnSearch.Name = "btnSearch";
            lbllstSessions = new Label();
            lbllstSessions.Name = "lbllstSessions";
            lstSessions = new ListView();
            lstSessions.Name = "lstSessions";
            btnSummary = new Button();
            btnSummary.Name = "btnSummary";
            btnFollowUp = new Button();
            btnFollowUp.Name = "btnFollowUp";
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
            lblHeading.Text = "Learners and saved sessions";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            lbltxtSearch.Text = "Search name, school, email, contact, degree or reference";
            lbltxtSearch.AutoSize = true;
            lbltxtSearch.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtSearch, 0, 2);
            txtSearch.Name = "txtSearch";
            txtSearch.AccessibleName = "Search name, school, email, contact, degree or reference";
            txtSearch.Dock = DockStyle.Fill;
            txtSearch.Margin = new Padding(5, 3, 5, 8);
            tblDialog.Controls.Add(txtSearch, 0, 3);
            lbldtpFrom.Text = "From (UTC)";
            lbldtpFrom.AutoSize = true;
            lbldtpFrom.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbldtpFrom, 0, 4);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.AccessibleName = "From (UTC)";
            dtpFrom.Dock = DockStyle.Fill;
            dtpFrom.Margin = new Padding(5, 3, 5, 8);
            tblDialog.Controls.Add(dtpFrom, 0, 5);
            lbldtpTo.Text = "To (UTC)";
            lbldtpTo.AutoSize = true;
            lbldtpTo.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbldtpTo, 0, 6);
            dtpTo.Name = "dtpTo";
            dtpTo.AccessibleName = "To (UTC)";
            dtpTo.Dock = DockStyle.Fill;
            dtpTo.Margin = new Padding(5, 3, 5, 8);
            tblDialog.Controls.Add(dtpTo, 0, 7);
            btnSearch.Name = "btnSearch";
            btnSearch.AccessibleName = "btnSearch";
            btnSearch.Dock = DockStyle.Fill;
            btnSearch.Margin = new Padding(5, 3, 5, 8);
            btnSearch.AutoSize = true;
            btnSearch.MinimumSize = new Size(0, 40);
            btnSearch.Text = "Search";
            btnSearch.Click += btnSearch_Click;
            tblDialog.Controls.Add(btnSearch, 0, 8);
            lbllstSessions.Text = "Saved sessions";
            lbllstSessions.AutoSize = true;
            lbllstSessions.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbllstSessions, 0, 9);
            lstSessions.Name = "lstSessions";
            lstSessions.AccessibleName = "Saved sessions";
            lstSessions.Dock = DockStyle.Fill;
            lstSessions.Margin = new Padding(5, 3, 5, 8);
            lstSessions.View = View.Details;
            lstSessions.FullRowSelect = true;
            lstSessions.MultiSelect = false;
            lstSessions.Height = 230;
            lstSessions.Columns.Add("Reference", 90);
            lstSessions.Columns.Add("Learner", 200);
            lstSessions.Columns.Add("Grade", 55);
            lstSessions.Columns.Add("Degree", 260);
            lstSessions.Columns.Add("Outcome", 190);
            lstSessions.Columns.Add("Date", 105);
            tblDialog.Controls.Add(lstSessions, 0, 10);
            btnSummary.Name = "btnSummary";
            btnSummary.AccessibleName = "btnSummary";
            btnSummary.Dock = DockStyle.Fill;
            btnSummary.Margin = new Padding(5, 3, 5, 8);
            btnSummary.AutoSize = true;
            btnSummary.MinimumSize = new Size(0, 40);
            btnSummary.Text = "View selected summary";
            btnSummary.Click += btnSummary_Click;
            tblDialog.Controls.Add(btnSummary, 0, 11);
            btnFollowUp.Name = "btnFollowUp";
            btnFollowUp.AccessibleName = "btnFollowUp";
            btnFollowUp.Dock = DockStyle.Fill;
            btnFollowUp.Margin = new Padding(5, 3, 5, 8);
            btnFollowUp.AutoSize = true;
            btnFollowUp.MinimumSize = new Size(0, 40);
            btnFollowUp.Text = "Create follow-up assessment";
            btnFollowUp.Click += btnFollowUp_Click;
            tblDialog.Controls.Add(btnFollowUp, 0, 12);
            tblDialog.RowCount = 13;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Learners and saved sessions";
            Name = "frmLearnerSessions";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
