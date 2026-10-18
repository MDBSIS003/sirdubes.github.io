#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmSubjectMarks
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblFields = null!;
        private Label lblcboSubject = null!;
        private ComboBox cboSubject = null!;
        private Label lblnudMark = null!;
        private NumericUpDown nudMark = null!;
        private Button btnAdd = null!;
        private Label lbllstMarks = null!;
        private ListView lstMarks = null!;
        private Button btnRemove = null!;
        private CheckBox chkComplete = null!;
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
            lblcboSubject = new Label();
            lblcboSubject.Name = "lblcboSubject";
            cboSubject = new ComboBox();
            cboSubject.Name = "cboSubject";
            lblnudMark = new Label();
            lblnudMark.Name = "lblnudMark";
            nudMark = new NumericUpDown();
            nudMark.Name = "nudMark";
            btnAdd = new Button();
            btnAdd.Name = "btnAdd";
            lbllstMarks = new Label();
            lbllstMarks.Name = "lbllstMarks";
            lstMarks = new ListView();
            lstMarks.Name = "lstMarks";
            btnRemove = new Button();
            btnRemove.Name = "btnRemove";
            chkComplete = new CheckBox();
            chkComplete.Name = "chkComplete";
            SuspendLayout();
            tblFields.AutoSize = true;
            tblFields.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblFields.ColumnCount = 1;
            tblFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblFields.Dock = DockStyle.Top;
            tblFields.Padding = new Padding(16);
            lblcboSubject.Text = "Subject *";
            lblcboSubject.AutoSize = true;
            lblcboSubject.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lblcboSubject, 0, 0);
            cboSubject.Name = "cboSubject";
            cboSubject.AccessibleName = "Subject *";
            cboSubject.Dock = DockStyle.Fill;
            cboSubject.Margin = new Padding(6, 3, 6, 8);
            cboSubject.MinimumSize = new Size(0, 30);
            cboSubject.DropDownStyle = ComboBoxStyle.DropDownList;
            tblFields.Controls.Add(cboSubject, 0, 1);
            lblnudMark.Text = "Mark (%) *";
            lblnudMark.AutoSize = true;
            lblnudMark.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lblnudMark, 0, 2);
            nudMark.Name = "nudMark";
            nudMark.AccessibleName = "Mark (%) *";
            nudMark.Dock = DockStyle.Fill;
            nudMark.Margin = new Padding(6, 3, 6, 8);
            nudMark.Maximum = 100;
            nudMark.DecimalPlaces = 2;
            tblFields.Controls.Add(nudMark, 0, 3);
            btnAdd.Name = "btnAdd";
            btnAdd.AccessibleName = "btnAdd";
            btnAdd.Dock = DockStyle.Fill;
            btnAdd.Margin = new Padding(6, 3, 6, 8);
            btnAdd.Text = "Add / update subject";
            btnAdd.AutoSize = true;
            btnAdd.Click += btnAdd_Click;
            tblFields.Controls.Add(btnAdd, 0, 4);
            lbllstMarks.Text = "Captured subjects and marks";
            lbllstMarks.AutoSize = true;
            lbllstMarks.Margin = new Padding(6, 10, 6, 3);
            tblFields.Controls.Add(lbllstMarks, 0, 5);
            lstMarks.Name = "lstMarks";
            lstMarks.AccessibleName = "Captured subjects and marks";
            lstMarks.Dock = DockStyle.Fill;
            lstMarks.Margin = new Padding(6, 3, 6, 8);
            lstMarks.Height = 235;
            lstMarks.View = View.Details;
            lstMarks.FullRowSelect = true;
            lstMarks.MultiSelect = false;
            lstMarks.Columns.Add("Subject", 450);
            lstMarks.Columns.Add("Mark (%)", 120);
            lstMarks.SelectedIndexChanged += lstMarks_SelectedIndexChanged;
            tblFields.Controls.Add(lstMarks, 0, 6);
            btnRemove.Name = "btnRemove";
            btnRemove.AccessibleName = "btnRemove";
            btnRemove.Dock = DockStyle.Fill;
            btnRemove.Margin = new Padding(6, 3, 6, 8);
            btnRemove.Text = "Remove selected subject";
            btnRemove.AutoSize = true;
            btnRemove.Click += btnRemove_Click;
            tblFields.Controls.Add(btnRemove, 0, 7);
            chkComplete.Name = "chkComplete";
            chkComplete.AccessibleName = "chkComplete";
            chkComplete.Dock = DockStyle.Fill;
            chkComplete.Margin = new Padding(6, 3, 6, 8);
            chkComplete.Text = "I confirm that this is my complete subject list";
            chkComplete.AutoSize = true;
            tblFields.Controls.Add(chkComplete, 0, 8);
            tblFields.RowCount = 9;
            pnlBody.Controls.Add(tblFields);
            Configure("Capture your subjects and marks", 2);
            Name = "frmSubjectMarks";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
