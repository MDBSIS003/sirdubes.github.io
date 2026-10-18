#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmAdvisorySummary
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblFields = null!;
        private Button btnExport = null!;
        private Button btnPrint = null!;
        private Button btnRetry = null!;
        private Label lblSummaryHeading = null!;
        private Label lblSummary = null!;
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
            tblFields = new TableLayoutPanel();
            tblFields.Name = "tblFields";
            btnExport = new Button();
            btnExport.Name = "btnExport";
            btnPrint = new Button();
            btnPrint.Name = "btnPrint";
            lblSummaryHeading = new Label();
            lblSummaryHeading.Name = "lblSummaryHeading";
            lblSummary = new Label();
            lblSummary.Name = "lblSummary";
            btnRetry = new Button();
            btnRetry.Name = "btnRetry";
            SuspendLayout();
            tblFields.AutoSize = true;
            tblFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblFields.ColumnCount = 1;
            tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFields.Dock = DockStyle.Top;
            tblFields.Padding = new Padding(16);
            btnExport.Name = "btnExport";
            btnExport.AccessibleName = "btnExport";
            btnExport.Dock = DockStyle.Fill;
            btnExport.Margin = new Padding(6, 3, 6, 8);
            btnExport.Text = "Save a copy";
            btnExport.AutoSize = true;
            btnExport.Click += btnExport_Click;
            tblFields.Controls.Add(btnExport, 0, 0);
            btnPrint.Name = "btnPrint";
            btnPrint.AccessibleName = "btnPrint";
            btnPrint.Dock = DockStyle.Fill;
            btnPrint.Margin = new Padding(6, 3, 6, 8);
            btnPrint.Text = "Print or save PDF";
            btnPrint.AutoSize = true;
            btnPrint.Click += btnPrint_Click;
            tblFields.Controls.Add(btnPrint, 0, 1);
            lblSummaryHeading.Text = "Your study pathway";
            lblSummaryHeading.AutoSize = true;
            lblSummaryHeading.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lblSummaryHeading, 0, 2);
            lblSummary.Name = "lblSummary";
            lblSummary.AccessibleName = "Your study pathway";
            lblSummary.Dock = DockStyle.Fill;
            lblSummary.Margin = new Padding(6, 3, 6, 8);
            lblSummary.MinimumSize = new Size(0, 30);
            lblSummary.AutoSize = true;
            lblSummary.BackColor = Color.White;
            lblSummary.Padding = new Padding(14);
            lblSummary.UseMnemonic = false;
            tblFields.Controls.Add(lblSummary, 0, 3);
            btnRetry.Text = "Retry summary";
            btnRetry.AutoSize = true;
            btnRetry.Dock = DockStyle.Fill;
            btnRetry.Margin = new Padding(6, 3, 6, 8);
            btnRetry.Visible = false;
            btnRetry.Click += btnRetry_Click;
            tblFields.Controls.Add(btnRetry, 0, 4);
            tblFields.RowCount = 5;
            pnlBody.Controls.Add(tblFields);
            Configure("Your saved advisory summary", 7);
            Name = "frmAdvisorySummary";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
