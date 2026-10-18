using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmEditInterestQuestion : frmStaffForm
    {
        public frmEditInterestQuestion()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        private int recordId;
        public frmEditInterestQuestion(AppController app, ReferenceRow? row) : this()
        {
            this.app = app;
            app.Access.RequireAdmin();
            recordId = row == null ? 0 : row.Id;
            if (row != null)
            {
                txtText.Text = row.Text("Text");
            }

            cboCategoryId.DisplayMember = "Name";
            cboCategoryId.ValueMember = "Id";
            cboCategoryId.DataSource = app.Maintenance.Lookup("InterestCategory");
            if (row != null)
            {
                cboCategoryId.SelectedValue = Convert.ToInt32(row.Values["CategoryId"]);
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
                values.Add("Text", txtText.Text);
                values.Add("CategoryId", cboCategoryId.SelectedValue);
                values.Add("Active", chkActive.Checked ? 1 : 0);
                app.Maintenance.Save("InterestQuestion", recordId, values);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
