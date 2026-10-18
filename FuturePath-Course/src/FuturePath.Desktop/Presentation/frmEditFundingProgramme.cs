using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmEditFundingProgramme : frmStaffForm
    {
        public frmEditFundingProgramme()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        private int recordId;
        public frmEditFundingProgramme(AppController app, ReferenceRow? row) : this()
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

            cboDegreeId.DisplayMember = "Name";
            cboDegreeId.ValueMember = "Id";
            cboDegreeId.DataSource = app.Maintenance.Lookup("DegreeProgramme");
            if (row != null)
            {
                cboDegreeId.SelectedValue = Convert.ToInt32(row.Values["DegreeId"]);
            }

            chkActive.Checked = row == null ? true : Convert.ToInt32(row.Values["Active"]) == 1;
            TrackChanges(this);
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                Dictionary<string, object?> values = new Dictionary<string, object?>();
                values.Add("FundingId", cboFundingId.SelectedValue);
                values.Add("DegreeId", cboDegreeId.SelectedValue);
                values.Add("Active", chkActive.Checked ? 1 : 0);
                app.Maintenance.Save("FundingProgramme", recordId, values);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
