using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmEditFundingOption : frmStaffForm
    {
        public frmEditFundingOption()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        private int recordId;
        public frmEditFundingOption(AppController app, ReferenceRow? row) : this()
        {
            this.app = app;
            app.Access.RequireAdmin();
            recordId = row == null ? 0 : row.Id;
            if (row != null)
            {
                txtName.Text = row.Text("Name");
            }

            cboType.Items.AddRange(ReferenceCatalog.FundingTypes);
            if (row != null)
            {
                cboType.SelectedItem = row.Text("Type");
            }

            if (row != null)
            {
                txtEligibility.Text = row.Text("Eligibility");
            }

            if (row != null)
            {
                txtDocuments.Text = row.Text("Documents");
            }

            dtpClosingDate.Value = row == null ? DateTime.Today.AddYears(1) : DateTime.Parse(row.Text("ClosingDate"), System.Globalization.CultureInfo.InvariantCulture);
            if (row != null)
            {
                txtContact.Text = row.Text("Contact");
            }

            chkSample.Checked = row == null ? true : Convert.ToInt32(row.Values["Sample"]) == 1;
            chkActive.Checked = row == null ? true : Convert.ToInt32(row.Values["Active"]) == 1;
            TrackChanges(this);
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                Dictionary<string, object?> values = new Dictionary<string, object?>();
                values.Add("Name", txtName.Text);
                values.Add("Type", cboType.Text);
                values.Add("Eligibility", txtEligibility.Text);
                values.Add("Documents", txtDocuments.Text);
                values.Add("ClosingDate", dtpClosingDate.Value.ToString("yyyy-MM-dd"));
                values.Add("Contact", txtContact.Text);
                values.Add("Sample", chkSample.Checked ? 1 : 0);
                values.Add("Active", chkActive.Checked ? 1 : 0);
                app.Maintenance.Save("FundingOption", recordId, values);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
