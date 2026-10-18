#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmAdvisoryResults
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblFields = null!;
        private Label lblOutcomeHeading = null!;
        private Label lblOutcomeDetails = null!;
        private Label lblpnlAlternatives = null!;
        private TableLayoutPanel pnlAlternatives = null!;
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
            lblOutcomeHeading = new Label();
            lblOutcomeHeading.Name = "lblOutcomeHeading";
            lblOutcomeDetails = new Label();
            lblOutcomeDetails.Name = "lblOutcomeDetails";
            lblpnlAlternatives = new Label();
            lblpnlAlternatives.Name = "lblpnlAlternatives";
            pnlAlternatives = new TableLayoutPanel();
            pnlAlternatives.Name = "pnlAlternatives";
            SuspendLayout();
            tblFields.AutoSize = true;
            tblFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblFields.ColumnCount = 1;
            tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFields.Dock = DockStyle.Top;
            tblFields.Padding = new Padding(16);
            lblOutcomeHeading.Text = "Selected programme and explanation";
            lblOutcomeHeading.AutoSize = true;
            lblOutcomeHeading.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblOutcomeHeading.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lblOutcomeHeading, 0, 0);
            lblOutcomeDetails.Name = "lblOutcomeDetails";
            lblOutcomeDetails.AccessibleName = "Selected programme and explanation";
            lblOutcomeDetails.Dock = DockStyle.Fill;
            lblOutcomeDetails.Margin = new Padding(6, 3, 6, 8);
            lblOutcomeDetails.MinimumSize = new Size(0, 30);
            lblOutcomeDetails.AutoSize = true;
            lblOutcomeDetails.BackColor = Color.White;
            lblOutcomeDetails.Padding = new Padding(14);
            lblOutcomeDetails.UseMnemonic = false;
            tblFields.Controls.Add(lblOutcomeDetails, 0, 1);
            lblpnlAlternatives.Text = "Ranked alternatives (eligible or near match)";
            lblpnlAlternatives.AutoSize = true;
            lblpnlAlternatives.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lblpnlAlternatives, 0, 2);
            pnlAlternatives.Name = "pnlAlternatives";
            pnlAlternatives.AccessibleName = "Ranked alternatives (eligible or near match)";
            pnlAlternatives.Dock = DockStyle.Fill;
            pnlAlternatives.Margin = new Padding(6, 3, 6, 8);
            pnlAlternatives.MinimumSize = new Size(0, 30);
            pnlAlternatives.AutoSize = true;
            pnlAlternatives.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlAlternatives.ColumnCount = 1;
            pnlAlternatives.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFields.Controls.Add(pnlAlternatives, 0, 3);
            tblFields.RowCount = 4;
            pnlBody.Controls.Add(tblFields);
            Configure("Your advisory outcome and alternatives", 5);
            Name = "frmAdvisoryResults";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
