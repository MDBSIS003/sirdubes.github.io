using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmPassword : frmStaffForm
    {
        public frmPassword()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        public frmPassword(AppController app) : this()
        {
            this.app = app;
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (txtNew.Text != txtConfirm.Text)
                {
                    throw new ValidationException("The new passwords do not match.");
                }

                app.Authentication.ChangePassword(txtCurrent.Text, txtNew.Text);
                MessageBox.Show(this, "Password updated.", "Future Path");
                Close();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
    }
}
