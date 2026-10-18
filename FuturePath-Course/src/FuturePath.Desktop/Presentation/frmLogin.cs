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
            ActiveControl = cboRole;
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cboRole.SelectedItem is not UserRole role)
                {
                    cboRole.Focus();
                    throw new ValidationException("Choose Advisor or Administrator before signing in.");
                }
                if (string.IsNullOrWhiteSpace(txtUsername.Text) || txtPassword.Text.Length == 0)
                {
                    throw new ValidationException("Enter your staff username and password.");
                }

                btnLogin.Enabled = false;
                app.Authentication.Login(txtUsername.Text, txtPassword.Text, role);
                txtPassword.Clear();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.ForeColor = Color.Firebrick;
                lblError.Text = ex.Message;
                txtPassword.Clear();
                if (cboRole.SelectedIndex < 0) { cboRole.Focus(); }
                else { txtPassword.Focus(); }
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }
    }
}
