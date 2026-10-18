#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmEditProgrammeRequirement
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lblcboDegreeId = null!;
        private ComboBox cboDegreeId = null!;
        private Label lblcboSubjectId = null!;
        private ComboBox cboSubjectId = null!;
        private Label lblnudMinimumMark = null!;
        private NumericUpDown nudMinimumMark = null!;
        private CheckBox chkMandatory = null!;
        private Label lblnudAlternativeGroup = null!;
        private NumericUpDown nudAlternativeGroup = null!;
        private Label lbltxtNote = null!;
        private TextBox txtNote = null!;
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
            lblcboSubjectId = new Label();
            lblcboSubjectId.Name = "lblcboSubjectId";
            cboSubjectId = new ComboBox();
            cboSubjectId.Name = "cboSubjectId";
            lblnudMinimumMark = new Label();
            lblnudMinimumMark.Name = "lblnudMinimumMark";
            nudMinimumMark = new NumericUpDown();
            nudMinimumMark.Name = "nudMinimumMark";
            chkMandatory = new CheckBox();
            chkMandatory.Name = "chkMandatory";
            lblnudAlternativeGroup = new Label();
            lblnudAlternativeGroup.Name = "lblnudAlternativeGroup";
            nudAlternativeGroup = new NumericUpDown();
            nudAlternativeGroup.Name = "nudAlternativeGroup";
            lbltxtNote = new Label();
            lbltxtNote.Name = "lbltxtNote";
            txtNote = new TextBox();
            txtNote.Name = "txtNote";
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
            lblHeading.Text = "Maintain ProgrammeRequirement";
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
            lblcboSubjectId.Text = "Subject";
            lblcboSubjectId.AutoSize = true;
            lblcboSubjectId.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblcboSubjectId, 0, 4);
            cboSubjectId.Name = "cboSubjectId";
            cboSubjectId.AccessibleName = "Subject";
            cboSubjectId.Dock = DockStyle.Fill;
            cboSubjectId.Margin = new Padding(5, 3, 5, 8);
            cboSubjectId.DropDownStyle = ComboBoxStyle.DropDownList;
            tblDialog.Controls.Add(cboSubjectId, 0, 5);
            lblnudMinimumMark.Text = "Minimum mark (%)";
            lblnudMinimumMark.AutoSize = true;
            lblnudMinimumMark.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblnudMinimumMark, 0, 6);
            nudMinimumMark.Name = "nudMinimumMark";
            nudMinimumMark.AccessibleName = "Minimum mark (%)";
            nudMinimumMark.Dock = DockStyle.Fill;
            nudMinimumMark.Margin = new Padding(5, 3, 5, 8);
            nudMinimumMark.Maximum = 100;
            nudMinimumMark.Minimum = 0;
            nudMinimumMark.DecimalPlaces = 2;
            tblDialog.Controls.Add(nudMinimumMark, 0, 7);
            chkMandatory.Name = "chkMandatory";
            chkMandatory.AccessibleName = "chkMandatory";
            chkMandatory.Dock = DockStyle.Fill;
            chkMandatory.Margin = new Padding(5, 3, 5, 8);
            chkMandatory.Text = "Mandatory";
            chkMandatory.AutoSize = true;
            tblDialog.Controls.Add(chkMandatory, 0, 8);
            lblnudAlternativeGroup.Text = "OR group (0 = independent)";
            lblnudAlternativeGroup.AutoSize = true;
            lblnudAlternativeGroup.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblnudAlternativeGroup, 0, 9);
            nudAlternativeGroup.Name = "nudAlternativeGroup";
            nudAlternativeGroup.AccessibleName = "OR group (0 = independent)";
            nudAlternativeGroup.Dock = DockStyle.Fill;
            nudAlternativeGroup.Margin = new Padding(5, 3, 5, 8);
            nudAlternativeGroup.Maximum = 99;
            nudAlternativeGroup.Minimum = 0;
            nudAlternativeGroup.DecimalPlaces = 0;
            tblDialog.Controls.Add(nudAlternativeGroup, 0, 10);
            lbltxtNote.Text = "Advisory note (optional)";
            lbltxtNote.AutoSize = true;
            lbltxtNote.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbltxtNote, 0, 11);
            txtNote.Name = "txtNote";
            txtNote.AccessibleName = "Advisory note (optional)";
            txtNote.Dock = DockStyle.Fill;
            txtNote.Margin = new Padding(5, 3, 5, 8);
            txtNote.MaxLength = 2000;
            txtNote.Multiline = true;
            txtNote.Height = 85;
            txtNote.ScrollBars = ScrollBars.Vertical;
            tblDialog.Controls.Add(txtNote, 0, 12);
            chkActive.Name = "chkActive";
            chkActive.AccessibleName = "chkActive";
            chkActive.Dock = DockStyle.Fill;
            chkActive.Margin = new Padding(5, 3, 5, 8);
            chkActive.Text = "Active";
            chkActive.AutoSize = true;
            tblDialog.Controls.Add(chkActive, 0, 13);
            btnSave.Name = "btnSave";
            btnSave.AccessibleName = "btnSave";
            btnSave.Dock = DockStyle.Fill;
            btnSave.Margin = new Padding(5, 3, 5, 8);
            btnSave.AutoSize = true;
            btnSave.MinimumSize = new Size(0, 40);
            btnSave.Text = "Save changes";
            btnSave.Click += btnSave_Click;
            tblDialog.Controls.Add(btnSave, 0, 14);
            tblDialog.RowCount = 15;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Maintain ProgrammeRequirement";
            Name = "frmEditProgrammeRequirement";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
