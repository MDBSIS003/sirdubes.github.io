using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmEditFundingFaculty : frmStaffForm
    {
        public frmEditFundingFaculty()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        private int recordId;
        public frmEditFundingFaculty(AppController app, ReferenceRow? row) : this()
        {
            this.app = app;
            app.Access.RequireAdmin();
            recordId = row == null ? 0 : row.Id;
            cboFundingId.DisplayMember = "Name";
            cboFundingId.ValueMember = "Id";
            cboFundingId.DataSource = app.Maintenance.Lookup("FundingOption");
            if (row != null)
            {
                cboFundingId.SelectedValue = Convert.ToInt32(row.Values["FundingId"]);
            }

            cboFacultyId.DisplayMember = "Name";
            cboFacultyId.ValueMember = "Id";
            cboFacultyId.DataSource = app.Maintenance.Lookup("Faculty");
            if (row != null)
            {
                cboFacultyId.SelectedValue = Convert.ToInt32(row.Values["FacultyId"]);
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
                values.Add("FundingId", cboFundingId.SelectedValue);
                values.Add("FacultyId", cboFacultyId.SelectedValue);
                values.Add("Active", chkActive.Checked ? 1 : 0);
                app.Maintenance.Save("FundingFaculty", recordId, values);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
