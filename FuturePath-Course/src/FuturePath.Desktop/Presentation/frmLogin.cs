using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmLogin : frmStaffForm
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        public frmLogin(AppController app) : this()
        {
            this.app = app;
            AcceptButton = btnLogin;
            ActiveControl = txtUsername;
        }

        private void btnCreateStaff_Click(object? sender, EventArgs e)
        {
            using (frmTesterRegistration registration = new frmTesterRegistration(app))
            {
                if (registration.ShowDialog(this) == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text) || txtPassword.Text.Length == 0)
                {
                    throw new ValidationException("Enter your staff username or email and password.");
                }

                btnLogin.Enabled = false;
                app.Authentication.Login(txtUsername.Text, txtPassword.Text);
                txtPassword.Clear();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.ForeColor = Color.Firebrick;
                lblError.Text = ex.Message;
                txtPassword.Clear();
                txtPassword.Focus();
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }
    }
}
