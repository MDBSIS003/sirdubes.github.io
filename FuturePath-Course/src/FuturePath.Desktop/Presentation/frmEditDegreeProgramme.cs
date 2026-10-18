using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmEditDegreeProgramme : frmStaffForm
    {
        public frmEditDegreeProgramme()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        private int recordId;
        public frmEditDegreeProgramme(AppController app, ReferenceRow? row) : this()
        {
            this.app = app;
            app.Access.RequireAdmin();
            recordId = row == null ? 0 : row.Id;
            if (row != null)
            {
                txtName.Text = row.Text("Name");
            }

            cboFacultyId.DisplayMember = "Name";
            cboFacultyId.ValueMember = "Id";
            cboFacultyId.DataSource = app.Maintenance.Lookup("Faculty");
            if (row != null)
            {
                cboFacultyId.SelectedValue = Convert.ToInt32(row.Values["FacultyId"]);
            }

            if (row != null)
            {
                txtDescription.Text = row.Text("Description");
            }

            if (row != null)
            {
                nudMinimumAverage.Value = Convert.ToDecimal(row.Values["MinimumAverage"]);
            }

            chkActive.Checked = row == null ? false : Convert.ToInt32(row.Values["Active"]) == 1;
            TrackChanges(this);
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                Dictionary<string, object?> values = new Dictionary<string, object?>();
                values.Add("Name", txtName.Text);
                values.Add("FacultyId", cboFacultyId.SelectedValue);
                values.Add("Description", txtDescription.Text);
                values.Add("MinimumAverage", nudMinimumAverage.Value);
                values.Add("Active", chkActive.Checked ? 1 : 0);
                app.Maintenance.Save("DegreeProgramme", recordId, values);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
