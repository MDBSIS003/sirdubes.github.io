#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmReportPreview
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Button btnSave = null!;
        private Button btnPrint = null!;
        private Label lbltxtReport = null!;
        private TextBox txtReport = null!;
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
            btnSave = new Button();
            btnSave.Name = "btnSave";
            btnPrint = new Button();
            btnPrint.Name = "btnPrint";
            lbltxtReport = new Label();
            lbltxtReport.Name = "lbltxtReport";
            txtReport = new TextBox();
            txtReport.Name = "txtReport";
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
            lblHeading.Text = "Report preview";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            btnSave.Name = "btnSave";
            btnSave.AccessibleName = "btnSave";
            btnSave.Dock = DockStyle.Fill;
            btnSave.Margin = new Padding(5, 3, 5, 8);
            btnSave.AutoSize = true;
            btnSave.MinimumSize = new Size(0, 40);
            btnSave.Text = "Save HTML / text";
            btnSave.Click += btnSave_Click;
            tblDialog.Controls.Add(btnSave, 0, 2);
            btnPrint.Name = "btnPrint";
            btnPrint.AccessibleName = "btnPrint";
            btnPrint.Dock = DockStyle.Fill;
            btnPrint.Margin = new Padding(5, 3, 5, 8);
            btnPrint.AutoSize = true;
            btnPrint.MinimumSize = new Size(0, 40);
            btnPrint.Text = "Print / Save as PDF";
            btnPrint.Click += btnPrint_Click;
            tblDialog.Controls.Add(btnPrint, 0, 3);
            lbltxtReport.Text = "Report content";
            lbltxtReport.AutoSize = true;
            lbltxtReport.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtReport, 0, 4);
            txtReport.Name = "txtReport";
            txtReport.AccessibleName = "Report content";
            txtReport.Dock = DockStyle.Fill;
            txtReport.Margin = new Padding(5, 3, 5, 8);
            txtReport.Multiline = true;
            txtReport.ReadOnly = true;
            txtReport.ScrollBars = ScrollBars.Vertical;
            txtReport.Height = 450;
            tblDialog.Controls.Add(txtReport, 0, 5);
            tblDialog.RowCount = 6;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Report preview";
            Name = "frmReportPreview";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
