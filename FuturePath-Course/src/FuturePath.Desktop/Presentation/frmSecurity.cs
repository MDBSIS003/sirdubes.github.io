using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmSecurity : frmStaffForm
    {
        public frmSecurity()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        public frmSecurity(AppController app) : this()
        {
            this.app = app;
            app.Access.RequireStaff();
            txtUsername.Text = app.Access.DisplayName;
            AcceptButton = btnSave;
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (txtNew.Text != txtConfirm.Text)
                {
                    throw new ValidationException("The new passwords do not match.");
                }

                app.Authentication.ChangeSecurity(txtUsername.Text, txtCurrent.Text, txtNew.Text);
                txtCurrent.Clear();
                txtNew.Clear();
                txtConfirm.Clear();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
