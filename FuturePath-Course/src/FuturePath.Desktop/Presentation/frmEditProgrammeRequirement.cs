using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmEditProgrammeRequirement : frmStaffForm
    {
        public frmEditProgrammeRequirement()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        private int recordId;
        public frmEditProgrammeRequirement(AppController app, ReferenceRow? row) : this()
        {
            this.app = app;
            app.Access.RequireAdmin();
            recordId = row == null ? 0 : row.Id;
            cboDegreeId.DisplayMember = "Name";
            cboDegreeId.ValueMember = "Id";
            cboDegreeId.DataSource = app.Maintenance.Lookup("DegreeProgramme");
            if (row != null)
            {
                cboDegreeId.SelectedValue = Convert.ToInt32(row.Values["DegreeId"]);
            }

            cboSubjectId.DisplayMember = "Name";
            cboSubjectId.ValueMember = "Id";
            cboSubjectId.DataSource = app.Maintenance.Lookup("Subject");
            if (row != null)
            {
                cboSubjectId.SelectedValue = Convert.ToInt32(row.Values["SubjectId"]);
            }

            if (row != null)
            {
                nudMinimumMark.Value = Convert.ToDecimal(row.Values["MinimumMark"]);
            }

            chkMandatory.Checked = row == null ? true : Convert.ToInt32(row.Values["Mandatory"]) == 1;
            if (row != null)
            {
                nudAlternativeGroup.Value = Convert.ToDecimal(row.Values["AlternativeGroup"]);
            }

            if (row != null)
            {
                txtNote.Text = row.Text("Note");
            }

            chkActive.Checked = row == null ? true : Convert.ToInt32(row.Values["Active"]) == 1;
            TrackChanges(this);
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (!ConfirmDeactivation()) { return; }

                Dictionary<string, object?> values = new Dictionary<string, object?>();
                values.Add("DegreeId", cboDegreeId.SelectedValue);
                values.Add("SubjectId", cboSubjectId.SelectedValue);
                values.Add("MinimumMark", nudMinimumMark.Value);
                values.Add("Mandatory", chkMandatory.Checked ? 1 : 0);
                values.Add("AlternativeGroup", nudAlternativeGroup.Value);
                values.Add("Note", txtNote.Text);
                values.Add("Active", chkActive.Checked ? 1 : 0);
                app.Maintenance.Save("ProgrammeRequirement", recordId, values);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
