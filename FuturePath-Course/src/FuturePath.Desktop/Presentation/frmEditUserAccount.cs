using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmEditUserAccount : frmStaffForm
    {
        public frmEditUserAccount()
        {
            InitializeComponent();
            cboRole.Items.AddRange(new object[] {
                UserRole.Advisor,
                UserRole.Administrator
            });
        }

        private AppController app = null!;
        private int recordId;
        public frmEditUserAccount(AppController app, ReferenceRow? row) : this()
        {
            this.app = app;
            app.Access.RequireAdmin();
            recordId = row == null ? 0 : row.Id;
            txtUsername.Text = row == null ? "" : row.Text("Username");
            chkActive.Checked = row == null || Convert.ToInt32(row.Values["Active"]) == 1;
            cboRole.SelectedItem = row == null ? UserRole.Advisor : Enum.Parse<UserRole>(row.Text("Role"));
            TrackChanges(this);
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cboRole.SelectedItem == null)
                {
                    throw new ValidationException("Choose a staff role.");
                }

                if (!ConfirmDeactivation()) { return; }

                app.Authentication.SaveUser(recordId, txtUsername.Text, (UserRole)cboRole.SelectedItem, chkActive.Checked, txtPassword.Text.Length == 0 ? null : txtPassword.Text);
                txtPassword.Clear();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
