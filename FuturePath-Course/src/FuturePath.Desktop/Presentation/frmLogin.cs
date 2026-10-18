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
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                app.Authentication.Login(txtUsername.Text, txtPassword.Text);
                txtPassword.Clear();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                txtPassword.Clear();
            }
        }
    }
}
