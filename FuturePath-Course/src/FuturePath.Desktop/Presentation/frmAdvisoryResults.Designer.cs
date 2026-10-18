#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmAdvisoryResults
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblFields = null!;
        private Label lbltxtOutcome = null!;
        private TextBox txtOutcome = null!;
        private Label lbltxtAlternatives = null!;
        private TextBox txtAlternatives = null!;
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
            lbltxtOutcome = new Label();
            lbltxtOutcome.Name = "lbltxtOutcome";
            txtOutcome = new TextBox();
            txtOutcome.Name = "txtOutcome";
            lbltxtAlternatives = new Label();
            lbltxtAlternatives.Name = "lbltxtAlternatives";
            txtAlternatives = new TextBox();
            txtAlternatives.Name = "txtAlternatives";
            SuspendLayout();
            tblFields.AutoSize = true;
            tblFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblFields.ColumnCount = 1;
            tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFields.Dock = DockStyle.Top;
            tblFields.Padding = new Padding(16);
            lbltxtOutcome.Text = "Selected programme and explanation";
            lbltxtOutcome.AutoSize = true;
            lbltxtOutcome.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lbltxtOutcome, 0, 0);
            txtOutcome.Name = "txtOutcome";
            txtOutcome.AccessibleName = "Selected programme and explanation";
            txtOutcome.Dock = DockStyle.Fill;
            txtOutcome.Margin = new Padding(6, 3, 6, 8);
            txtOutcome.MinimumSize = new Size(0, 30);
            txtOutcome.Multiline = true;
            txtOutcome.ReadOnly = true;
            txtOutcome.ScrollBars = ScrollBars.Vertical;
            txtOutcome.Height = 250;
            tblFields.Controls.Add(txtOutcome, 0, 1);
            lbltxtAlternatives.Text = "Ranked alternatives (eligible or near match)";
            lbltxtAlternatives.AutoSize = true;
            lbltxtAlternatives.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lbltxtAlternatives, 0, 2);
            txtAlternatives.Name = "txtAlternatives";
            txtAlternatives.AccessibleName = "Ranked alternatives (eligible or near match)";
            txtAlternatives.Dock = DockStyle.Fill;
            txtAlternatives.Margin = new Padding(6, 3, 6, 8);
            txtAlternatives.MinimumSize = new Size(0, 30);
            txtAlternatives.Multiline = true;
            txtAlternatives.ReadOnly = true;
            txtAlternatives.ScrollBars = ScrollBars.Vertical;
            txtAlternatives.Height = 250;
            tblFields.Controls.Add(txtAlternatives, 0, 3);
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
