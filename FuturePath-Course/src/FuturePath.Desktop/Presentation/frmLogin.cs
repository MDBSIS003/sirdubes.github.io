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

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text) || txtPassword.Text.Length == 0)
                {
                    throw new ValidationException("Enter your staff username and password.");
                }

                btnLogin.Enabled = false;
                app.Authentication.Login(txtUsername.Text, txtPassword.Text);
                txtPassword.Clear();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
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
