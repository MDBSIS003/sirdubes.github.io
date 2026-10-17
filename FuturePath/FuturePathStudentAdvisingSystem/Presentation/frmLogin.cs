using FuturePathStudentAdvisingSystem.DataAccess;
using FuturePathStudentAdvisingSystem.Security;
using FuturePathStudentAdvisingSystem.UI;

namespace FuturePathStudentAdvisingSystem
{
    /// <summary>
    /// Login screen for Student (Learner), Career Advisor and Administrator.
    /// Returns DialogResult.OK after a successful login (AppSession is then set),
    /// or Cancel if the user goes back. Only students see "Create an account".
    /// </summary>
    public class frmLogin : BrandedForm
    {
        private readonly string role;
        private readonly FieldBox fldUser;
        private readonly FieldBox fldPassword;
        private readonly CheckBox chkShow;
        private readonly Label lblMessage;
        private readonly Button btnLogin;

        public frmLogin(string role)
            : base("Future Path - " + Theme.RoleTitle(role) + " login", Theme.Navy,
                   Theme.RoleColor(role), Theme.RoleGlyph(role),
                   Theme.RoleTitle(role), Theme.RoleBlurb(role))
        {
            this.role = role;
            bool isStudent = role == Roles.Learner;
            const int left = 75;
            const int width = 420;

            Label back = Theme.LinkLabel("\u2190  Back", 24, 20);
            back.Click += (s, e) => DialogResult = DialogResult.Cancel;
            Content.Controls.Add(back);

            Content.Controls.Add(Theme.Heading("Welcome back", left, 66));
            Content.Controls.Add(Theme.SubHeading("Log in to continue as " + Theme.RoleTitle(role) + ".", left + 2, 110));

            Content.Controls.Add(Theme.Caption(isStudent ? "Email" : "Username", left, 154));
            fldUser = FieldBox.ForText(isStudent ? Theme.GlyphMail : Theme.GlyphPerson, width, false, 50);
            fldUser.Location = new Point(left, 176);
            Content.Controls.Add(fldUser);

            Content.Controls.Add(Theme.Caption("Password", left, 228));
            fldPassword = FieldBox.ForText(Theme.GlyphLock, width, true, 128);
            fldPassword.Location = new Point(left, 250);
            Content.Controls.Add(fldPassword);

            chkShow = new CheckBox
            {
                Text = "Show password",
                AutoSize = true,
                Location = new Point(left, 298),
                Font = Theme.Body(9.5F),
                ForeColor = Theme.Muted
            };
            chkShow.CheckedChanged += (s, e) =>
                fldPassword.TextBox.UseSystemPasswordChar = !chkShow.Checked;
            Content.Controls.Add(chkShow);

            lblMessage = new Label
            {
                AutoSize = false,
                Location = new Point(left, 326),
                Size = new Size(width, 36),
                Font = Theme.Body(9.5F),
                ForeColor = Theme.Danger
            };
            Content.Controls.Add(lblMessage);

            btnLogin = Theme.PrimaryButton("Log in", Theme.Navy, width, 44);
            btnLogin.Location = new Point(left, 368);
            btnLogin.Click += btnLogin_Click;
            Content.Controls.Add(btnLogin);
            AcceptButton = btnLogin;

            if (isStudent)
            {
                Label or = new Label
                {
                    Text = "or",
                    AutoSize = false,
                    Location = new Point(left, 424),
                    Size = new Size(width, 20),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Theme.Muted
                };
                Content.Controls.Add(or);

                Button create = Theme.OutlineButton("Create an account", Theme.TealDark, width, 42);
                create.Location = new Point(left, 448);
                create.Click += btnCreateAccount_Click;
                Content.Controls.Add(create);
            }
            else
            {
                Label note = new Label
                {
                    Text = "Staff accounts are set up by the system administrator.",
                    AutoSize = false,
                    Location = new Point(left, 428),
                    Size = new Size(width, 20),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = Theme.Body(9F),
                    ForeColor = Theme.Muted
                };
                Content.Controls.Add(note);
            }

            Shown += (s, e) => fldUser.TextBox.Focus();
        }

        private void ShowError(string message)
        {
            lblMessage.ForeColor = Theme.Danger;
            lblMessage.Text = message;
        }

        private void ShowInfo(string message)
        {
            lblMessage.ForeColor = Theme.TealDark;
            lblMessage.Text = message;
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            string username = fldUser.TextBox.Text.Trim();
            string password = fldPassword.TextBox.Text;

            if (username.Length == 0 || password.Length == 0)
            {
                ShowError(role == Roles.Learner
                    ? "Please enter your email and password."
                    : "Please enter your username and password.");
                return;
            }

            try
            {
                UserAccountDAL dal = new UserAccountDAL();

                if (!dal.TryLogin(username, password, role, out int userID, out string displayName))
                {
                    // One generic message: don't reveal whether the username or the password was wrong.
                    ShowError("Incorrect login details for this account type.");
                    fldPassword.TextBox.Clear();
                    fldPassword.TextBox.Focus();
                    return;
                }

                AppSession.Start(userID, role, displayName);
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not log in. Please check the database connection.\n\n" + ex.Message,
                    "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateAccount_Click(object? sender, EventArgs e)
        {
            using (frmCreateAccount create = new frmCreateAccount())
            {
                if (create.ShowDialog(this) == DialogResult.OK)
                {
                    fldUser.TextBox.Text = create.CreatedEmail;
                    fldPassword.TextBox.Clear();
                    ShowInfo("Account created. Please log in.");
                    fldPassword.TextBox.Focus();
                }
            }
        }
    }
}
