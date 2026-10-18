using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmEditFaculty : frmStaffForm
    {
        public frmEditFaculty()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        private int recordId;
        public frmEditFaculty(AppController app, ReferenceRow? row) : this()
        {
            this.app = app;
            app.Access.RequireAdmin();
            recordId = row == null ? 0 : row.Id;
            if (row != null)
            {
                txtName.Text = row.Text("Name");
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
                values.Add("Name", txtName.Text);
                values.Add("Active", chkActive.Checked ? 1 : 0);
                app.Maintenance.Save("Faculty", recordId, values);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
