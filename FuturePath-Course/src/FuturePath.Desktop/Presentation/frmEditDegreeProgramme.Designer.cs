#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmEditDegreeProgramme
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lbltxtName = null!;
        private TextBox txtName = null!;
        private Label lblcboFacultyId = null!;
        private ComboBox cboFacultyId = null!;
        private Label lbltxtDescription = null!;
        private TextBox txtDescription = null!;
        private Label lblnudMinimumAverage = null!;
        private NumericUpDown nudMinimumAverage = null!;
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
            lbltxtName = new Label();
            lbltxtName.Name = "lbltxtName";
            txtName = new TextBox();
            txtName.Name = "txtName";
            lblcboFacultyId = new Label();
            lblcboFacultyId.Name = "lblcboFacultyId";
            cboFacultyId = new ComboBox();
            cboFacultyId.Name = "cboFacultyId";
            lbltxtDescription = new Label();
            lbltxtDescription.Name = "lbltxtDescription";
            txtDescription = new TextBox();
            txtDescription.Name = "txtDescription";
            lblnudMinimumAverage = new Label();
            lblnudMinimumAverage.Name = "lblnudMinimumAverage";
            nudMinimumAverage = new NumericUpDown();
            nudMinimumAverage.Name = "nudMinimumAverage";
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
            lblHeading.Text = "Maintain DegreeProgramme";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            lbltxtName.Text = "Degree name";
            lbltxtName.AutoSize = true;
            lbltxtName.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtName, 0, 2);
            txtName.Name = "txtName";
            txtName.AccessibleName = "Degree name";
            txtName.Dock = DockStyle.Fill;
            txtName.Margin = new Padding(5, 3, 5, 8);
            txtName.MaxLength = 150;
            tblDialog.Controls.Add(txtName, 0, 3);
            lblcboFacultyId.Text = "Faculty";
            lblcboFacultyId.AutoSize = true;
            lblcboFacultyId.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblcboFacultyId, 0, 4);
            cboFacultyId.Name = "cboFacultyId";
            cboFacultyId.AccessibleName = "Faculty";
            cboFacultyId.Dock = DockStyle.Fill;
            cboFacultyId.Margin = new Padding(5, 3, 5, 8);
            cboFacultyId.DropDownStyle = ComboBoxStyle.DropDownList;
            tblDialog.Controls.Add(cboFacultyId, 0, 5);
            lbltxtDescription.Text = "Description";
            lbltxtDescription.AutoSize = true;
            lbltxtDescription.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtDescription, 0, 6);
            txtDescription.Name = "txtDescription";
            txtDescription.AccessibleName = "Description";
            txtDescription.Dock = DockStyle.Fill;
            txtDescription.Margin = new Padding(5, 3, 5, 8);
            txtDescription.MaxLength = 2000;
            txtDescription.Multiline = true;
            txtDescription.Height = 85;
            txtDescription.ScrollBars = ScrollBars.Vertical;
            tblDialog.Controls.Add(txtDescription, 0, 7);
            lblnudMinimumAverage.Text = "Minimum overall average (%)";
            lblnudMinimumAverage.AutoSize = true;
            lblnudMinimumAverage.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblnudMinimumAverage, 0, 8);
            nudMinimumAverage.Name = "nudMinimumAverage";
            nudMinimumAverage.AccessibleName = "Minimum overall average (%)";
            nudMinimumAverage.Dock = DockStyle.Fill;
            nudMinimumAverage.Margin = new Padding(5, 3, 5, 8);
            nudMinimumAverage.Maximum = 100;
            nudMinimumAverage.Minimum = 0;
            nudMinimumAverage.DecimalPlaces = 2;
            tblDialog.Controls.Add(nudMinimumAverage, 0, 9);
            chkActive.Name = "chkActive";
            chkActive.AccessibleName = "chkActive";
            chkActive.Dock = DockStyle.Fill;
            chkActive.Margin = new Padding(5, 3, 5, 8);
            chkActive.Text = "Active";
            chkActive.AutoSize = true;
            tblDialog.Controls.Add(chkActive, 0, 10);
            btnSave.Name = "btnSave";
            btnSave.AccessibleName = "btnSave";
            btnSave.Dock = DockStyle.Fill;
            btnSave.Margin = new Padding(5, 3, 5, 8);
            btnSave.AutoSize = true;
            btnSave.MinimumSize = new Size(0, 40);
            btnSave.Text = "Save changes";
            btnSave.Click += btnSave_Click;
            tblDialog.Controls.Add(btnSave, 0, 11);
            tblDialog.RowCount = 12;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Maintain DegreeProgramme";
            Name = "frmEditDegreeProgramme";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
