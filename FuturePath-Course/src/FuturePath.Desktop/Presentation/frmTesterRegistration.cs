using System;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmTesterRegistration : frmStaffForm
    {
        private AppController app = null!;

        public frmTesterRegistration()
        {
            InitializeComponent();
            cboRole.Items.AddRange(new object[] { UserRole.Advisor, UserRole.Administrator });
            cboRole.SelectedItem = UserRole.Administrator;
        }

        public frmTesterRegistration(AppController app) : this()
        {
            this.app = app;
            AcceptButton = btnRequest;
        }

        private void SetPending(bool pending)
        {
            txtEmail.ReadOnly = txtUsername.ReadOnly = pending;
            txtPassword.Enabled = txtConfirm.Enabled = cboRole.Enabled = !pending;
            btnRequest.Enabled = !pending;
            btnCode.Enabled = btnEdit.Enabled = pending;
            AcceptButton = pending ? btnCode : btnRequest;
        }

        private void btnRequest_Click(object? sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text != txtConfirm.Text)
                {
                    throw new ValidationException("The passwords do not match.");
                }
                if (cboRole.SelectedItem is not UserRole role)
                {
                    throw new ValidationException("Choose a staff role.");
                }
                app.Authentication.RequestTesterRegistration(txtEmail.Text, txtUsername.Text, txtPassword.Text, role);
                txtPassword.Clear();
                txtConfirm.Clear();
                SetPending(true);
                lblMessage.ForeColor = Color.DarkGreen;
                lblMessage.Text = "Test code requested. No email is sent. Select Code inserted (test) within five minutes to create your account and sign in.";
                btnCode.Focus();
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = Color.Firebrick;
                lblMessage.Text = ex.Message;
            }
        }

        private void btnCode_Click(object? sender, EventArgs e)
        {
            btnCode.Enabled = false;
            try
            {
                app.Authentication.CompleteTesterRegistration();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                app.Authentication.CancelTesterRegistration();
                SetPending(false);
                lblMessage.ForeColor = Color.Firebrick;
                lblMessage.Text = ex.Message;
            }
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            app.Authentication.CancelTesterRegistration();
            SetPending(false);
            lblMessage.Text = "Update your details and request another test code.";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (app != null) { app.Authentication.CancelTesterRegistration(); }
            base.OnFormClosed(e);
        }
    }
}
