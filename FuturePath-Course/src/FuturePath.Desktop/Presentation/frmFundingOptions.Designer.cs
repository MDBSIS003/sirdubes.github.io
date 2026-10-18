#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmFundingOptions
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblFields = null!;
        private CheckBox chkFunding = null!;
        private Label lbllblFunding = null!;
        private Label lblFunding = null!;
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
            lbllblFunding = new Label();
            lbllblFunding.Name = "lbllblFunding";
            lblFunding = new Label();
            lblFunding.Name = "lblFunding";
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
            lbllblFunding.Text = "Linked funding opportunities";
            lbllblFunding.AutoSize = true;
            lbllblFunding.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lbllblFunding, 0, 1);
            lblFunding.Name = "lblFunding";
            lblFunding.AccessibleName = "Linked funding opportunities";
            lblFunding.Dock = DockStyle.Fill;
            lblFunding.Margin = new Padding(6, 3, 6, 8);
            lblFunding.MinimumSize = new Size(0, 30);
            lblFunding.AutoSize = true;
            lblFunding.BackColor = Color.White;
            lblFunding.Padding = new Padding(14);
            lblFunding.UseMnemonic = false;
            tblFields.Controls.Add(lblFunding, 0, 2);
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
