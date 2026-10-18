#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmLearnerProfile
    {
        private System.ComponentModel.IContainer? components;
        private ErrorProvider errProfile = null!;
        private TableLayoutPanel tblFields = null!;
        private Label lbltxtName = null!;
        private TextBox txtName = null!;
        private Label lbltxtSchool = null!;
        private TextBox txtSchool = null!;
        private Label lblcboProvince = null!;
        private ComboBox cboProvince = null!;
        private Label lblcboGrade = null!;
        private ComboBox cboGrade = null!;
        private Label lbltxtEmail = null!;
        private TextBox txtEmail = null!;
        private Label lbltxtPhone = null!;
        private TextBox txtPhone = null!;
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
            lbltxtName = new Label();
            lbltxtName.Name = "lbltxtName";
            txtName = new TextBox();
            txtName.Name = "txtName";
            lbltxtSchool = new Label();
            lbltxtSchool.Name = "lbltxtSchool";
            txtSchool = new TextBox();
            txtSchool.Name = "txtSchool";
            lblcboProvince = new Label();
            lblcboProvince.Name = "lblcboProvince";
            cboProvince = new ComboBox();
            cboProvince.Name = "cboProvince";
            lblcboGrade = new Label();
            lblcboGrade.Name = "lblcboGrade";
            cboGrade = new ComboBox();
            cboGrade.Name = "cboGrade";
            lbltxtEmail = new Label();
            lbltxtEmail.Name = "lbltxtEmail";
            txtEmail = new TextBox();
            txtEmail.Name = "txtEmail";
            lbltxtPhone = new Label();
            lbltxtPhone.Name = "lbltxtPhone";
            txtPhone = new TextBox();
            txtPhone.Name = "txtPhone";
            errProfile = new ErrorProvider(components);
            errProfile.ContainerControl = this;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
            SuspendLayout();
            tblFields.AutoSize = true;
            tblFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblFields.ColumnCount = 1;
            tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFields.Dock = DockStyle.Top;
            tblFields.Padding = new Padding(16);
            lbltxtName.Text = "Name and surname *";
            lbltxtName.AutoSize = true;
            lbltxtName.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lbltxtName, 0, 0);
            txtName.Name = "txtName";
            txtName.AccessibleName = "Name and surname *";
            txtName.Dock = DockStyle.Fill;
            txtName.Margin = new Padding(6, 3, 6, 8);
            txtName.MinimumSize = new Size(0, 30);
            txtName.MaxLength = 120;
            tblFields.Controls.Add(txtName, 0, 1);
            lbltxtSchool.Text = "School *";
            lbltxtSchool.AutoSize = true;
            lbltxtSchool.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lbltxtSchool, 0, 2);
            txtSchool.Name = "txtSchool";
            txtSchool.AccessibleName = "School *";
            txtSchool.Dock = DockStyle.Fill;
            txtSchool.Margin = new Padding(6, 3, 6, 8);
            txtSchool.MinimumSize = new Size(0, 30);
            txtSchool.MaxLength = 120;
            txtSchool.TextChanged += ProfileChanged;
            tblFields.Controls.Add(txtSchool, 0, 3);
            lblcboProvince.Text = "Province *";
            lblcboProvince.AutoSize = true;
            lblcboProvince.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lblcboProvince, 0, 4);
            cboProvince.Name = "cboProvince";
            cboProvince.AccessibleName = "Province *";
            cboProvince.Dock = DockStyle.Fill;
            cboProvince.Margin = new Padding(6, 3, 6, 8);
            cboProvince.MinimumSize = new Size(0, 30);
            cboProvince.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProvince.SelectedIndexChanged += ProfileChanged;
            tblFields.Controls.Add(cboProvince, 0, 5);
            lblcboGrade.Text = "Grade *";
            lblcboGrade.AutoSize = true;
            lblcboGrade.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lblcboGrade, 0, 6);
            cboGrade.Name = "cboGrade";
            cboGrade.AccessibleName = "Grade *";
            cboGrade.Dock = DockStyle.Fill;
            cboGrade.Margin = new Padding(6, 3, 6, 8);
            cboGrade.MinimumSize = new Size(0, 30);
            cboGrade.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGrade.Items.AddRange(new object[] {
                "11",
                "12"
            });
            cboGrade.SelectedIndexChanged += ProfileChanged;
            tblFields.Controls.Add(cboGrade, 0, 7);
            lbltxtEmail.Text = "Email *";
            lbltxtEmail.AutoSize = true;
            lbltxtEmail.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lbltxtEmail, 0, 8);
            txtEmail.Name = "txtEmail";
            txtEmail.AccessibleName = "Email *";
            txtEmail.Dock = DockStyle.Fill;
            txtEmail.Margin = new Padding(6, 3, 6, 8);
            txtEmail.MinimumSize = new Size(0, 30);
            txtEmail.MaxLength = 150;
            tblFields.Controls.Add(txtEmail, 0, 9);
            lbltxtPhone.Text = "Contact number (10 digits) *";
            lbltxtPhone.AutoSize = true;
            lbltxtPhone.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lbltxtPhone, 0, 10);
            txtPhone.Name = "txtPhone";
            txtPhone.AccessibleName = "Contact number (10 digits) *";
            txtPhone.Dock = DockStyle.Fill;
            txtPhone.Margin = new Padding(6, 3, 6, 8);
            txtPhone.MinimumSize = new Size(0, 30);
            txtPhone.MaxLength = 10;
            tblFields.Controls.Add(txtPhone, 0, 11);
            tblFields.RowCount = 12;
            pnlBody.Controls.Add(tblFields);
            Configure("Capture your learner profile", 1);
            Name = "frmLearnerProfile";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
