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
        private Label lbltxtSummary = null!;
        private TextBox txtSummary = null!;
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
            lbltxtSummary = new Label();
            lbltxtSummary.Name = "lbltxtSummary";
            txtSummary = new TextBox();
            txtSummary.Name = "txtSummary";
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
            btnExport.Text = "Save report (HTML / text)";
            btnExport.AutoSize = true;
            btnExport.Click += btnExport_Click;
            tblFields.Controls.Add(btnExport, 0, 0);
            btnPrint.Name = "btnPrint";
            btnPrint.AccessibleName = "btnPrint";
            btnPrint.Dock = DockStyle.Fill;
            btnPrint.Margin = new Padding(6, 3, 6, 8);
            btnPrint.Text = "Print / Save as PDF";
            btnPrint.AutoSize = true;
            btnPrint.Click += btnPrint_Click;
            tblFields.Controls.Add(btnPrint, 0, 1);
            lbltxtSummary.Text = "Saved assessment";
            lbltxtSummary.AutoSize = true;
            lbltxtSummary.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lbltxtSummary, 0, 2);
            txtSummary.Name = "txtSummary";
            txtSummary.AccessibleName = "Saved assessment";
            txtSummary.Dock = DockStyle.Fill;
            txtSummary.Margin = new Padding(6, 3, 6, 8);
            txtSummary.MinimumSize = new Size(0, 30);
            txtSummary.Multiline = true;
            txtSummary.ReadOnly = true;
            txtSummary.ScrollBars = ScrollBars.Vertical;
            txtSummary.Height = 450;
            tblFields.Controls.Add(txtSummary, 0, 3);
            tblFields.RowCount = 4;
            pnlBody.Controls.Add(tblFields);
            Configure("Your saved advisory summary", 7);
            Name = "frmAdvisorySummary";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
