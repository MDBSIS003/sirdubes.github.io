#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmEditDegreeInterestMapping
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lblcboDegreeId = null!;
        private ComboBox cboDegreeId = null!;
        private Label lblcboCategoryId = null!;
        private ComboBox cboCategoryId = null!;
        private Label lblnudWeight = null!;
        private NumericUpDown nudWeight = null!;
        private CheckBox chkActive = null!;
        private Button btnSave = null!;
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
            lblcboDegreeId = new Label();
            lblcboDegreeId.Name = "lblcboDegreeId";
            cboDegreeId = new ComboBox();
            cboDegreeId.Name = "cboDegreeId";
            lblcboCategoryId = new Label();
            lblcboCategoryId.Name = "lblcboCategoryId";
            cboCategoryId = new ComboBox();
            cboCategoryId.Name = "cboCategoryId";
            lblnudWeight = new Label();
            lblnudWeight.Name = "lblnudWeight";
            nudWeight = new NumericUpDown();
            nudWeight.Name = "nudWeight";
            chkActive = new CheckBox();
            chkActive.Name = "chkActive";
            btnSave = new Button();
            btnSave.Name = "btnSave";
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
            lblHeading.Text = "Maintain DegreeInterestMapping";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            lblcboDegreeId.Text = "Degree";
            lblcboDegreeId.AutoSize = true;
            lblcboDegreeId.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblcboDegreeId, 0, 2);
            cboDegreeId.Name = "cboDegreeId";
            cboDegreeId.AccessibleName = "Degree";
            cboDegreeId.Dock = DockStyle.Fill;
            cboDegreeId.Margin = new Padding(5, 3, 5, 8);
            cboDegreeId.DropDownStyle = ComboBoxStyle.DropDownList;
            tblDialog.Controls.Add(cboDegreeId, 0, 3);
            lblcboCategoryId.Text = "Category";
            lblcboCategoryId.AutoSize = true;
            lblcboCategoryId.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblcboCategoryId, 0, 4);
            cboCategoryId.Name = "cboCategoryId";
            cboCategoryId.AccessibleName = "Category";
            cboCategoryId.Dock = DockStyle.Fill;
            cboCategoryId.Margin = new Padding(5, 3, 5, 8);
            cboCategoryId.DropDownStyle = ComboBoxStyle.DropDownList;
            tblDialog.Controls.Add(cboCategoryId, 0, 5);
            lblnudWeight.Text = "Weight (0.01 to 10)";
            lblnudWeight.AutoSize = true;
            lblnudWeight.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblnudWeight, 0, 6);
            nudWeight.Name = "nudWeight";
            nudWeight.AccessibleName = "Weight (0.01 to 10)";
            nudWeight.Dock = DockStyle.Fill;
            nudWeight.Margin = new Padding(5, 3, 5, 8);
            nudWeight.Maximum = 10;
            nudWeight.Minimum = .01m;
            nudWeight.DecimalPlaces = 2;
            nudWeight.Value = 1;
            tblDialog.Controls.Add(nudWeight, 0, 7);
            chkActive.Name = "chkActive";
            chkActive.AccessibleName = "chkActive";
            chkActive.Dock = DockStyle.Fill;
            chkActive.Margin = new Padding(5, 3, 5, 8);
            chkActive.Text = "Active";
            chkActive.AutoSize = true;
            tblDialog.Controls.Add(chkActive, 0, 8);
            btnSave.Name = "btnSave";
            btnSave.AccessibleName = "btnSave";
            btnSave.Dock = DockStyle.Fill;
            btnSave.Margin = new Padding(5, 3, 5, 8);
            btnSave.AutoSize = true;
            btnSave.MinimumSize = new Size(0, 40);
            btnSave.Text = "Save changes";
            btnSave.Click += btnSave_Click;
            tblDialog.Controls.Add(btnSave, 0, 9);
            tblDialog.RowCount = 10;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Maintain DegreeInterestMapping";
            Name = "frmEditDegreeInterestMapping";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
