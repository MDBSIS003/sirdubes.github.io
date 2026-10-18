#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmAdvisoryStep
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblPage = null!;
        protected Label lblTitle = null!;
        protected Label lblMessage = null!;
        protected Panel pnlBody = null!;
        private TableLayoutPanel tblFooter = null!;
        protected Button btnBack = null!;
        protected Button btnNext = null!;
        protected Button btnEnd = null!;
        private Label lblStep = null!;
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
            tblPage = new TableLayoutPanel();
            tblPage.Name = "tblPage";
            lblTitle = new Label();
            lblTitle.Name = "lblTitle";
            lblMessage = new Label();
            lblMessage.Name = "lblMessage";
            pnlBody = new Panel();
            pnlBody.Name = "pnlBody";
            tblFooter = new TableLayoutPanel();
            tblFooter.Name = "tblFooter";
            btnBack = new Button();
            btnBack.Name = "btnBack";
            btnNext = new Button();
            btnNext.Name = "btnNext";
            btnEnd = new Button();
            btnEnd.Name = "btnEnd";
            lblStep = new Label();
            lblStep.Name = "lblStep";
            SuspendLayout();
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            BackColor = Color.FromArgb(244, 247, 249);
            ClientSize = new Size(1100, 720);
            FormBorderStyle = FormBorderStyle.None;
            ControlBox = false;
            tblPage.Dock = DockStyle.Fill;
            tblPage.ColumnCount = 1;
            tblPage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblPage.RowCount = 4;
            tblPage.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tblPage.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tblPage.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tblPage.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            lblTitle.AutoSize = true;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Padding = new Padding(20, 14, 20, 8);
            lblTitle.ForeColor = Color.FromArgb(22, 45, 63);
            lblMessage.AutoSize = true;
            lblMessage.Dock = DockStyle.Fill;
            lblMessage.Padding = new Padding(20, 4, 20, 8);
            lblMessage.ForeColor = Color.Firebrick;
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.AutoScroll = true;
            tblFooter.AutoSize = true;
            tblFooter.Dock = DockStyle.Fill;
            tblFooter.ColumnCount = 4;
            tblFooter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tblFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFooter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tblFooter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tblFooter.Padding = new Padding(12);
            btnBack.Text = "Back";
            btnNext.Text = "Next";
            btnEnd.Text = "End session";
            btnBack.AutoSize = btnNext.AutoSize = btnEnd.AutoSize = true;
            btnBack.MinimumSize = btnNext.MinimumSize = btnEnd.MinimumSize = new Size(105, 40);
            btnBack.Click += btnBack_Click;
            btnNext.Click += btnNext_Click;
            btnEnd.Click += btnEnd_Click;
            btnNext.BackColor = Color.FromArgb(15, 112, 100);
            btnNext.ForeColor = Color.White;
            lblStep.AutoSize = true;
            lblStep.Dock = DockStyle.Fill;
            lblStep.TextAlign = ContentAlignment.MiddleCenter;
            tblFooter.Controls.Add(btnBack, 0, 0);
            tblFooter.Controls.Add(lblStep, 1, 0);
            tblFooter.Controls.Add(btnNext, 2, 0);
            tblFooter.Controls.Add(btnEnd, 3, 0);
            tblPage.Controls.Add(lblTitle, 0, 0);
            tblPage.Controls.Add(lblMessage, 0, 1);
            tblPage.Controls.Add(pnlBody, 0, 2);
            tblPage.Controls.Add(tblFooter, 0, 3);
            Controls.Add(tblPage);
            Name = "frmAdvisoryStep";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
