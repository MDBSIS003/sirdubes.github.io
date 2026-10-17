using System.Net.Mail;
using Microsoft.Data.SqlClient;
using FuturePathStudentAdvisingSystem.DataAccess;
using FuturePathStudentAdvisingSystem.UI;

namespace FuturePathStudentAdvisingSystem
{
    /// <summary>
    /// Student sign-up. Saves a Learner row in UserAccount (the email is the username).
    /// Returns DialogResult.OK with CreatedEmail set, or Cancel if the user goes back.
    /// </summary>
    public class frmCreateAccount : BrandedForm
    {
        private static readonly string[] Provinces =
        {
            "Eastern Cape", "Free State", "Gauteng", "KwaZulu-Natal", "Limpopo",
            "Mpumalanga", "Northern Cape", "North West", "Western Cape"
        };

        private readonly FieldBox fldFirstName;
        private readonly FieldBox fldLastName;
        private readonly FieldBox fldEmail;
        private readonly FieldBox fldSchool;
        private readonly FieldBox fldProvince;
        private readonly FieldBox fldPassword;
        private readonly FieldBox fldConfirm;
        private readonly Label lblMessage;

        public string CreatedEmail { get; private set; } = string.Empty;

        public frmCreateAccount()
            : base("Future Path - Create an account", Theme.TealDark, Theme.Navy, Theme.GlyphAddUser,
                   "Join Future Path", "Save your results and come back to them any time.")
        {
            const int colA = 45;
            const int colB = 295;
            const int half = 230;
            const int full = 480;

            Label back = Theme.LinkLabel("\u2190  Back to login", 24, 20);
            back.Click += (s, e) => DialogResult = DialogResult.Cancel;
            Content.Controls.Add(back);

            Content.Controls.Add(Theme.Heading("Create your account", colA, 56));
            Content.Controls.Add(Theme.SubHeading("All fields are required.", colA + 2, 100));

            Content.Controls.Add(Theme.Caption("First name", colA, 134));
            fldFirstName = FieldBox.ForText(Theme.GlyphPerson, half, false, 50);
            fldFirstName.Location = new Point(colA, 154);

            Content.Controls.Add(Theme.Caption("Last name", colB, 134));
            fldLastName = FieldBox.ForText(Theme.GlyphPerson, half, false, 50);
            fldLastName.Location = new Point(colB, 154);

            Content.Controls.Add(Theme.Caption("Email (this is your username)", colA, 206));
            fldEmail = FieldBox.ForText(Theme.GlyphMail, full, false, 50);
            fldEmail.Location = new Point(colA, 226);

            Content.Controls.Add(Theme.Caption("School", colA, 278));
            fldSchool = FieldBox.ForText(Theme.GlyphSchool, half, false, 100);
            fldSchool.Location = new Point(colA, 298);

            Content.Controls.Add(Theme.Caption("Province", colB, 278));
            fldProvince = FieldBox.ForCombo(Theme.GlyphLocation, half, Provinces);
            fldProvince.Location = new Point(colB, 298);

            Content.Controls.Add(Theme.Caption("Password (10 or more characters)", colA, 350));
            fldPassword = FieldBox.ForText(Theme.GlyphLock, half, true, 128);
            fldPassword.Location = new Point(colA, 370);

            Content.Controls.Add(Theme.Caption("Confirm password", colB, 350));
            fldConfirm = FieldBox.ForText(Theme.GlyphLock, half, true, 128);
            fldConfirm.Location = new Point(colB, 370);

            foreach (FieldBox field in new[] { fldFirstName, fldLastName, fldEmail, fldSchool,
                                               fldProvince, fldPassword, fldConfirm })
                Content.Controls.Add(field);

            lblMessage = new Label
            {
                AutoSize = false,
                Location = new Point(colA, 416),
                Size = new Size(full, 22),
                Font = Theme.Body(9.5F),
                ForeColor = Theme.Danger
            };
            Content.Controls.Add(lblMessage);

            Button create = Theme.PrimaryButton("Create account", Theme.Teal, full, 44);
            create.Location = new Point(colA, 446);
            create.Click += btnCreate_Click;
            Content.Controls.Add(create);
            AcceptButton = create;

            Shown += (s, e) => fldFirstName.TextBox.Focus();
        }

        private bool Fail(string message, Control toFocus)
        {
            lblMessage.Text = message;
            toFocus.Focus();
            return false;
        }

        /// <summary>Required fields, formats and lengths. Returns true when everything is valid.</summary>
        private bool ValidateForm()
        {
            lblMessage.Text = string.Empty;

            string first = fldFirstName.TextBox.Text.Trim();
            string last = fldLastName.TextBox.Text.Trim();
            string email = fldEmail.TextBox.Text.Trim();
            string school = fldSchool.TextBox.Text.Trim();

            if (first.Length == 0) return Fail("Please enter your first name.", fldFirstName.TextBox);
            if (last.Length == 0) return Fail("Please enter your last name.", fldLastName.TextBox);
            if (email.Length == 0) return Fail("Please enter your email address.", fldEmail.TextBox);
            if (!IsValidEmail(email)) return Fail("Please enter a valid email address.", fldEmail.TextBox);
            if (school.Length == 0) return Fail("Please enter your school.", fldSchool.TextBox);
            if (fldProvince.ComboBox.SelectedIndex == -1) return Fail("Please select your province.", fldProvince.ComboBox);
            if (fldPassword.TextBox.Text.Length < 10) return Fail("Your password must be at least 10 characters.", fldPassword.TextBox);
            if (fldPassword.TextBox.Text != fldConfirm.TextBox.Text)
            {
                fldConfirm.TextBox.Clear();
                return Fail("The passwords do not match.", fldConfirm.TextBox);
            }

            return true;
        }

        private void btnCreate_Click(object? sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            string email = fldEmail.TextBox.Text.Trim();

            try
            {
                UserAccountDAL dal = new UserAccountDAL();

                if (dal.EmailExists(email))
                {
                    Fail("An account with this email already exists.", fldEmail.TextBox);
                    return;
                }

                dal.CreateStudentAccount(
                    fldFirstName.TextBox.Text.Trim(),
                    fldLastName.TextBox.Text.Trim(),
                    email,
                    fldSchool.TextBox.Text.Trim(),
                    fldProvince.ComboBox.Text,
                    fldPassword.TextBox.Text);

                CreatedEmail = email;
                DialogResult = DialogResult.OK;
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                // Unique constraint on Username: someone created the same email a moment ago.
                Fail("An account with this email already exists.", fldEmail.TextBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create the account.\n\n" + ex.Message,
                    "Create Account Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress address = new MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
