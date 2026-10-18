using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmEditDegreeInterestMapping : frmStaffForm
    {
        public frmEditDegreeInterestMapping()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        private int recordId;
        public frmEditDegreeInterestMapping(AppController app, ReferenceRow? row) : this()
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

            cboCategoryId.DisplayMember = "Name";
            cboCategoryId.ValueMember = "Id";
            cboCategoryId.DataSource = app.Maintenance.Lookup("InterestCategory");
            if (row != null)
            {
                cboCategoryId.SelectedValue = Convert.ToInt32(row.Values["CategoryId"]);
            }

            if (row != null)
            {
                nudWeight.Value = Convert.ToDecimal(row.Values["Weight"]);
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
                values.Add("CategoryId", cboCategoryId.SelectedValue);
                values.Add("Weight", nudWeight.Value);
                values.Add("Active", chkActive.Checked ? 1 : 0);
                app.Maintenance.Save("DegreeInterestMapping", recordId, values);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
