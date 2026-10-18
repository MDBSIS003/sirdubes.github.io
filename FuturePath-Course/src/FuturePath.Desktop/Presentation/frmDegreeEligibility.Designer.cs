#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmDegreeEligibility
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblFields = null!;
        private Label lblcboFaculty = null!;
        private ComboBox cboFaculty = null!;
        private Label lblcboDegree = null!;
        private ComboBox cboDegree = null!;
        private Label lbllblRequirements = null!;
        private Label lblRequirements = null!;
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
            lblcboFaculty = new Label();
            lblcboFaculty.Name = "lblcboFaculty";
            cboFaculty = new ComboBox();
            cboFaculty.Name = "cboFaculty";
            lblcboDegree = new Label();
            lblcboDegree.Name = "lblcboDegree";
            cboDegree = new ComboBox();
            cboDegree.Name = "cboDegree";
            lbllblRequirements = new Label();
            lbllblRequirements.Name = "lbllblRequirements";
            lblRequirements = new Label();
            lblRequirements.Name = "lblRequirements";
            SuspendLayout();
            tblFields.AutoSize = true;
            tblFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblFields.ColumnCount = 1;
            tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFields.Dock = DockStyle.Top;
            tblFields.Padding = new Padding(16);
            lblcboFaculty.Text = "Preferred faculty *";
            lblcboFaculty.AutoSize = true;
            lblcboFaculty.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lblcboFaculty, 0, 0);
            cboFaculty.Name = "cboFaculty";
            cboFaculty.AccessibleName = "Preferred faculty *";
            cboFaculty.Dock = DockStyle.Fill;
            cboFaculty.Margin = new Padding(6, 3, 6, 8);
            cboFaculty.MinimumSize = new Size(0, 30);
            cboFaculty.DropDownStyle = ComboBoxStyle.DropDownList;
            cboFaculty.SelectedIndexChanged += cboFaculty_SelectedIndexChanged;
            tblFields.Controls.Add(cboFaculty, 0, 1);
            lblcboDegree.Text = "Degree programme *";
            lblcboDegree.AutoSize = true;
            lblcboDegree.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lblcboDegree, 0, 2);
            cboDegree.Name = "cboDegree";
            cboDegree.AccessibleName = "Degree programme *";
            cboDegree.Dock = DockStyle.Fill;
            cboDegree.Margin = new Padding(6, 3, 6, 8);
            cboDegree.MinimumSize = new Size(0, 30);
            cboDegree.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDegree.SelectedIndexChanged += cboDegree_SelectedIndexChanged;
            tblFields.Controls.Add(cboDegree, 0, 3);
            lbllblRequirements.Text = "Current programme requirements";
            lbllblRequirements.AutoSize = true;
            lbllblRequirements.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lbllblRequirements, 0, 4);
            lblRequirements.Name = "lblRequirements";
            lblRequirements.AccessibleName = "Current programme requirements";
            lblRequirements.Dock = DockStyle.Fill;
            lblRequirements.Margin = new Padding(6, 3, 6, 8);
            lblRequirements.MinimumSize = new Size(0, 30);
            lblRequirements.AutoSize = true;
            lblRequirements.BackColor = Color.White;
            lblRequirements.Padding = new Padding(14);
            lblRequirements.UseMnemonic = false;
            tblFields.Controls.Add(lblRequirements, 0, 5);
            tblFields.RowCount = 6;
            pnlBody.Controls.Add(tblFields);
            Configure("Select a faculty and degree", 4);
            Name = "frmDegreeEligibility";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
