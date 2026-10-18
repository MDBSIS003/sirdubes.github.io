#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmInterestQuestionnaire
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblFields = null!;
        private Label lblpnlQuestions = null!;
        private TableLayoutPanel pnlQuestions = null!;
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
            lblpnlQuestions = new Label();
            lblpnlQuestions.Name = "lblpnlQuestions";
            pnlQuestions = new TableLayoutPanel();
            pnlQuestions.Name = "pnlQuestions";
            SuspendLayout();
            tblFields.AutoSize = true;
            tblFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblFields.ColumnCount = 1;
            tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFields.Dock = DockStyle.Top;
            tblFields.Padding = new Padding(16);
            lblpnlQuestions.Text = "Choose one response per question";
            lblpnlQuestions.AutoSize = true;
            lblpnlQuestions.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lblpnlQuestions, 0, 0);
            pnlQuestions.Name = "pnlQuestions";
            pnlQuestions.AccessibleName = "Choose one response per question";
            pnlQuestions.Dock = DockStyle.Fill;
            pnlQuestions.Margin = new Padding(6, 3, 6, 8);
            pnlQuestions.AutoSize = true;
            pnlQuestions.ColumnCount = 1;
            pnlQuestions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFields.Controls.Add(pnlQuestions, 0, 1);
            tblFields.RowCount = 2;
            pnlBody.Controls.Add(tblFields);
            Configure("Explore your interests", 3);
            Name = "frmInterestQuestionnaire";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
