using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmAdminMaintenance : frmStaffForm
    {
        public frmAdminMaintenance()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        public frmAdminMaintenance(AppController app) : this()
        {
            this.app = app;
            app.Access.RequireAdmin();
            cboArea.SelectedIndex = 0;
        }

        private void cboArea_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (app == null)
            {
                return;
            }

            try
            {
                string key = cboArea.Text;
                List<ReferenceRow> rows = app.Maintenance.Rows(key);
                lstRecords.Clear();
                lstRecords.Columns.Add("Id", 60);
                List<string> fields = new List<string>();
                if (rows.Count > 0)
                {
                    foreach (string field in rows[0].Values.Keys)
                    {
                        fields.Add(field);
                    }
                }
                else if (key != "UserAccount" && key != "AuditEvent")
                {
                    foreach (ReferenceField field in ReferenceCatalog.Get(key).Fields)
                    {
                        fields.Add(field.Name);
                    }
                }

                foreach (string field in fields)
                {
                    lstRecords.Columns.Add(field, 180);
                }

                foreach (ReferenceRow row in rows)
                {
                    ListViewItem item = new ListViewItem(row.Id.ToString());
                    foreach (string field in fields)
                    {
                        item.SubItems.Add(row.Text(field));
                    }

                    item.Tag = row;
                    lstRecords.Items.Add(item);
                }

                btnAdd.Enabled = btnEdit.Enabled = key != "AuditEvent";
                lblError.Text = "Create new degrees as inactive. Add requirements and interest weights, then activate. Deactivate records instead of deleting history.";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void EditRecord(ReferenceRow? row)
        {
            Form? editor = null;
            switch (cboArea.Text)
            {
                case "Faculty":
                    editor = new frmEditFaculty(app, row);
                    break;
                case "Subject":
                    editor = new frmEditSubject(app, row);
                    break;
                case "InterestCategory":
                    editor = new frmEditInterestCategory(app, row);
                    break;
                case "DegreeProgramme":
                    editor = new frmEditDegreeProgramme(app, row);
                    break;
                case "ProgrammeRequirement":
                    editor = new frmEditProgrammeRequirement(app, row);
                    break;
                case "InterestQuestion":
                    editor = new frmEditInterestQuestion(app, row);
                    break;
                case "DegreeInterestMapping":
                    editor = new frmEditDegreeInterestMapping(app, row);
                    break;
                case "FundingOption":
                    editor = new frmEditFundingOption(app, row);
                    break;
                case "FundingFaculty":
                    editor = new frmEditFundingFaculty(app, row);
                    break;
                case "FundingProgramme":
                    editor = new frmEditFundingProgramme(app, row);
                    break;
                case "UserAccount":
                    editor = new frmEditUserAccount(app, row);
                    break;
            }

            if (editor == null)
            {
                return;
            }

            using (editor)
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                {
                    cboArea_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            EditRecord(null);
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            if (lstRecords.SelectedItems.Count == 0)
            {
                lblError.Text = "Select a record.";
                return;
            }

            EditRecord((ReferenceRow)lstRecords.SelectedItems[0].Tag!);
        }

        private void btnBackup_Click(object? sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "SQL Server backup (*.bak)|*.bak";
                dialog.FileName = "FuturePath-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".bak";
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    app.Maintenance.Backup(dialog.FileName);
                    lblError.Text = "Backup created and verified.";
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                }
            }
        }
    }
}
