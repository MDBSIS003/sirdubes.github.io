using FuturePathStudentAdvisingSystem.DataAccess;
using FuturePathStudentAdvisingSystem.Security;

namespace FuturePathStudentAdvisingSystem
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();

            Load += frmLogin_Load;
            rdoGuest.CheckedChanged += RoleChanged;
            rdoLearner.CheckedChanged += RoleChanged;
            rdoAdvisor.CheckedChanged += RoleChanged;
            rdoAdmin.CheckedChanged += RoleChanged;
            chkShowPassword.CheckedChanged += (s, e) =>
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            btnLogin.Click += btnLogin_Click;
            btnExit.Click += (s, e) => Close();
        }

        private void frmLogin_Load(object? sender, EventArgs e)
        {
            rdoGuest.Checked = true;
            UpdateCredentialState();
        }

        private void RoleChanged(object? sender, EventArgs e)
        {
            UpdateCredentialState();
        }

        private string SelectedRole()
        {
            if (rdoLearner.Checked) return Roles.Learner;
            if (rdoAdvisor.Checked) return Roles.Advisor;
            if (rdoAdmin.Checked) return Roles.Admin;
            return Roles.Guest;
        }

        /// <summary>Guests don't need credentials, so grey the fields out and rename the button.</summary>
        private void UpdateCredentialState()
        {
            bool needsLogin = SelectedRole() != Roles.Guest;

            txtUsername.Enabled = needsLogin;
            txtPassword.Enabled = needsLogin;
            chkShowPassword.Enabled = needsLogin;
            lblUsername.Enabled = needsLogin;
            lblPassword.Enabled = needsLogin;

            btnLogin.Text = needsLogin ? "Log In" : "Continue as Guest";
            lblMessage.Text = string.Empty;

            if (!needsLogin)
            {
                txtUsername.Clear();
                txtPassword.Clear();
            }
            else
            {
                txtUsername.Focus();
            }
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            string role = SelectedRole();

            // ---- Guest: no credentials, no database lookup ----
            if (role == Roles.Guest)
            {
                AppSession.StartGuest();
                OpenHomeScreen();
                return;
            }

            // ---- Validation (required fields) ----
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (username.Length == 0 || password.Length == 0)
            {
                lblMessage.Text = "Please enter your username and password.";
                return;
            }

            // ---- Authenticate against UserAccount ----
            try
            {
                UserAccountDAL dal = new UserAccountDAL();

                if (!dal.TryLogin(username, password, role, out int userID, out string displayName))
                {
                    // One generic message: don't reveal whether the username or password was wrong.
                    lblMessage.Text = "Incorrect username or password for the selected role.";
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                AppSession.Start(userID, role, displayName);
                OpenHomeScreen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not log in. Please check the database connection.\n\n" + ex.Message,
                    "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Sends each role to its own starting screen. Hides the login form while the user is
        /// working and shows it again when they close the screen (this is also "log out").
        /// </summary>
        private void OpenHomeScreen()
        {
            Hide();

            switch (AppSession.Role)
            {
                case Roles.Guest:
                case Roles.Learner:
                    // Existing learner flow starts at the profile screen.
                    using (Form1 learnerStart = new Form1())
                        learnerStart.ShowDialog();
                    break;

                case Roles.Advisor:
                    // TODO: replace with the Career Advisor home screen when it exists.
                    MessageBox.Show("Career Advisor screen not built yet.\nLogged in as: " + AppSession.DisplayName,
                        "Coming soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case Roles.Admin:
                    // TODO: replace with the Administrator home screen when it exists.
                    MessageBox.Show("Administrator screen not built yet.\nLogged in as: " + AppSession.DisplayName,
                        "Coming soon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            AppSession.End();
            txtPassword.Clear();
            rdoGuest.Checked = true;
            UpdateCredentialState();
            Show();
        }
    }
}
