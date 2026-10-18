#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmDatabaseSetup
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private TextBox txtInstructions = null!;
        private Button btnPrepare = null!;
        private Button btnRetry = null!;
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
            txtInstructions = new TextBox();
            txtInstructions.Name = "txtInstructions";
            btnPrepare = new Button();
            btnPrepare.Name = "btnPrepare";
            btnRetry = new Button();
            btnRetry.Name = "btnRetry";
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
            lblHeading.Text = "Prepare your local database";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            txtInstructions.Name = "txtInstructions";
            txtInstructions.AccessibleName = "txtInstructions";
            txtInstructions.Dock = DockStyle.Fill;
            txtInstructions.Margin = new Padding(5, 3, 5, 8);
            txtInstructions.Multiline = true;
            txtInstructions.ReadOnly = true;
            txtInstructions.ScrollBars = ScrollBars.Vertical;
            txtInstructions.Height = 230;
            tblDialog.Controls.Add(txtInstructions, 0, 2);
            btnPrepare.Name = "btnPrepare";
            btnPrepare.AccessibleName = "btnPrepare";
            btnPrepare.Dock = DockStyle.Fill;
            btnPrepare.Margin = new Padding(5, 3, 5, 8);
            btnPrepare.AutoSize = true;
            btnPrepare.MinimumSize = new Size(0, 40);
            btnPrepare.Text = "Prepare demonstration database";
            btnPrepare.Click += btnPrepare_Click;
            tblDialog.Controls.Add(btnPrepare, 0, 3);
            btnRetry.Name = "btnRetry";
            btnRetry.AccessibleName = "btnRetry";
            btnRetry.Dock = DockStyle.Fill;
            btnRetry.Margin = new Padding(5, 3, 5, 8);
            btnRetry.AutoSize = true;
            btnRetry.MinimumSize = new Size(0, 40);
            btnRetry.Text = "Retry connection";
            btnRetry.Click += btnRetry_Click;
            tblDialog.Controls.Add(btnRetry, 0, 4);
            tblDialog.RowCount = 5;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Prepare your local database";
            Name = "frmDatabaseSetup";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
