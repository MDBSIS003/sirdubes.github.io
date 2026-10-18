#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmFundingOptions
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblFields = null!;
        private CheckBox chkFunding = null!;
        private Label lbltxtFunding = null!;
        private TextBox txtFunding = null!;
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
            chkFunding = new CheckBox();
            chkFunding.Name = "chkFunding";
            lbltxtFunding = new Label();
            lbltxtFunding.Name = "lbltxtFunding";
            txtFunding = new TextBox();
            txtFunding.Name = "txtFunding";
            SuspendLayout();
            tblFields.AutoSize = true;
            tblFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblFields.ColumnCount = 1;
            tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFields.Dock = DockStyle.Top;
            tblFields.Padding = new Padding(16);
            chkFunding.Name = "chkFunding";
            chkFunding.AccessibleName = "chkFunding";
            chkFunding.Dock = DockStyle.Fill;
            chkFunding.Margin = new Padding(6, 3, 6, 8);
            chkFunding.Text = "I would like funding information";
            chkFunding.AutoSize = true;
            chkFunding.CheckedChanged += chkFunding_CheckedChanged;
            tblFields.Controls.Add(chkFunding, 0, 0);
            lbltxtFunding.Text = "Linked funding opportunities";
            lbltxtFunding.AutoSize = true;
            lbltxtFunding.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lbltxtFunding, 0, 1);
            txtFunding.Name = "txtFunding";
            txtFunding.AccessibleName = "Linked funding opportunities";
            txtFunding.Dock = DockStyle.Fill;
            txtFunding.Margin = new Padding(6, 3, 6, 8);
            txtFunding.MinimumSize = new Size(0, 30);
            txtFunding.Multiline = true;
            txtFunding.ReadOnly = true;
            txtFunding.ScrollBars = ScrollBars.Vertical;
            txtFunding.Height = 400;
            tblFields.Controls.Add(txtFunding, 0, 2);
            tblFields.RowCount = 3;
            pnlBody.Controls.Add(tblFields);
            Configure("Explore funding and save your assessment", 6);
            Name = "frmFundingOptions";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
