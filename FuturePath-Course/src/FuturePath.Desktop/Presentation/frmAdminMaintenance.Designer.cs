#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class frmAdminMaintenance
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblDialog = null!;
        private Panel pnlScroll = null!;
        private Label lblHeading = null!;
        private Label lblError = null!;
        private Label lblcboArea = null!;
        private ComboBox cboArea = null!;
        private Label lbllstRecords = null!;
        private ListView lstRecords = null!;
        private Button btnAdd = null!;
        private Button btnEdit = null!;
        private Button btnBackup = null!;
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
            lblcboArea = new Label();
            lblcboArea.Name = "lblcboArea";
            cboArea = new ComboBox();
            cboArea.Name = "cboArea";
            lbllstRecords = new Label();
            lbllstRecords.Name = "lbllstRecords";
            lstRecords = new ListView();
            lstRecords.Name = "lstRecords";
            btnAdd = new Button();
            btnAdd.Name = "btnAdd";
            btnEdit = new Button();
            btnEdit.Name = "btnEdit";
            btnBackup = new Button();
            btnBackup.Name = "btnBackup";
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
            lblHeading.Text = "Administrator maintenance";
            lblError.AutoSize = true;
            lblError.Dock = DockStyle.Fill;
            lblError.ForeColor = Color.Firebrick;
            tblDialog.Controls.Add(lblHeading, 0, 0);
            tblDialog.Controls.Add(lblError, 0, 1);
            lblcboArea.Text = "Reference area";
            lblcboArea.AutoSize = true;
            lblcboArea.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lblcboArea, 0, 2);
            cboArea.Name = "cboArea";
            cboArea.AccessibleName = "Reference area";
            cboArea.Dock = DockStyle.Fill;
            cboArea.Margin = new Padding(5, 3, 5, 8);
            cboArea.DropDownStyle = ComboBoxStyle.DropDownList;
            cboArea.Items.AddRange(new object[] {
                "Faculty",
                "Subject",
                "InterestCategory",
                "DegreeProgramme",
                "ProgrammeRequirement",
                "InterestQuestion",
                "DegreeInterestMapping",
                "FundingOption",
                "FundingFaculty",
                "FundingProgramme",
                "UserAccount",
                "AuditEvent"
            });
            cboArea.SelectedIndexChanged += cboArea_SelectedIndexChanged;
            tblDialog.Controls.Add(cboArea, 0, 3);
            lbllstRecords.Text = "Records";
            lbllstRecords.AutoSize = true;
            lbllstRecords.Margin = new Padding(5, 10, 5, 3);
            tblDialog.Controls.Add(lbllstRecords, 0, 4);
            lstRecords.Name = "lstRecords";
            lstRecords.AccessibleName = "Records";
            lstRecords.Dock = DockStyle.Fill;
            lstRecords.Margin = new Padding(5, 3, 5, 8);
            lstRecords.View = View.Details;
            lstRecords.FullRowSelect = true;
            lstRecords.MultiSelect = false;
            lstRecords.Height = 260;
            tblDialog.Controls.Add(lstRecords, 0, 5);
            btnAdd.Name = "btnAdd";
            btnAdd.AccessibleName = "btnAdd";
            btnAdd.Dock = DockStyle.Fill;
            btnAdd.Margin = new Padding(5, 3, 5, 8);
            btnAdd.AutoSize = true;
            btnAdd.MinimumSize = new Size(0, 40);
            btnAdd.Text = "Add record";
            btnAdd.Click += btnAdd_Click;
            tblDialog.Controls.Add(btnAdd, 0, 6);
            btnEdit.Name = "btnEdit";
            btnEdit.AccessibleName = "btnEdit";
            btnEdit.Dock = DockStyle.Fill;
            btnEdit.Margin = new Padding(5, 3, 5, 8);
            btnEdit.AutoSize = true;
            btnEdit.MinimumSize = new Size(0, 40);
            btnEdit.Text = "Edit selected record / deactivate";
            btnEdit.Click += btnEdit_Click;
            tblDialog.Controls.Add(btnEdit, 0, 7);
            btnBackup.Name = "btnBackup";
            btnBackup.AccessibleName = "btnBackup";
            btnBackup.Dock = DockStyle.Fill;
            btnBackup.Margin = new Padding(5, 3, 5, 8);
            btnBackup.AutoSize = true;
            btnBackup.MinimumSize = new Size(0, 40);
            btnBackup.Text = "Create verified database backup (.bak)";
            btnBackup.Click += btnBackup_Click;
            tblDialog.Controls.Add(btnBackup, 0, 8);
            tblDialog.RowCount = 9;
            pnlScroll.Controls.Add(tblDialog);
            Controls.Add(pnlScroll);
            Text = "Future Path | Administrator maintenance";
            Name = "frmAdminMaintenance";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
