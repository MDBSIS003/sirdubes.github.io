using System;
using System.Net.Mail;
using System.Windows.Forms;
using FuturePathStudentAdvisingSystem.DataAccess;
using FuturePathStudentAdvisingSystem.Security;

namespace FuturePathStudentAdvisingSystem
{
    public partial class Form1 : Form
    {
        private int savedUserID = 0;

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
            btnNext.Click += btnNext_Click;
        }



        private void Form1_Load(object? sender, EventArgs e)
        {
            // Test the database connection first
            bool connected =
                FuturePathStudentAdvisingSystem.DataAccess.DatabaseConnection
                .TestConnection(out string message);

            if (!connected)
            {
                MessageBox.Show(
                    message,
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // Load provinces
            cmbProvince.Items.Clear();
            cmbProvince.Items.AddRange(new string[]
            {
        "Eastern Cape",
        "Free State",
        "Gauteng",
        "KwaZulu-Natal",
        "Limpopo",
        "Mpumalanga",
        "Northern Cape",
        "North West",
        "Western Cape"
            });

            // Load grade levels from the database
            GradeLevelDAL gradeDAL = new GradeLevelDAL();
            cmbGrade.DataSource = gradeDAL.GetGradeLevels();
            cmbGrade.DisplayMember = "GradeName";
            cmbGrade.ValueMember = "GradeLevelID";
            cmbGrade.SelectedIndex = -1;

            ApplySessionToProfile();
        }


        private void btnNext_Click(object? sender, EventArgs e)
        {
            // Guests only choose a grade: nothing personal is collected or saved.
            if (AppSession.IsInRole(Roles.Guest))
            {
                if (cmbGrade.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select your current grade.");
                    return;
                }

                OpenSubjectsForm();
                return;
            }

            // Logged-in students already have a saved profile.
            if (savedUserID > 0)
            {
                if (cmbGrade.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select your current grade.");
                    return;
                }

                OpenSubjectsForm();
                return;
            }

            if (string.IsNullOrWhiteSpace(textFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtSchoolName.Text) ||
                cmbProvince.SelectedIndex == -1 ||
                cmbGrade.SelectedIndex == -1)
            {
                MessageBox.Show("Please complete all required fields.");
                return;
            }

            string email = txtEmail.Text.Trim();

            if (email.Length > 50)
            {
                MessageBox.Show("Email must not exceed 50 characters.");
                txtEmail.Focus();
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Please enter a valid email address.");
                txtEmail.Focus();
                return;
            }

            try
            {
                LearnerProfileDAL profileDAL = new LearnerProfileDAL();

                savedUserID = profileDAL.SaveLearnerProfile(
                    textFirstName.Text.Trim(),
                    txtLastName.Text.Trim(),
                    email,
                    txtSchoolName.Text.Trim(),
                    cmbProvince.Text
                );

                MessageBox.Show(
                    "Learner profile saved successfully.\nLearner ID: "
                    + savedUserID,
                    "Profile Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                OpenSubjectsForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not save the learner profile.\n\n" + ex.Message,
                    "Save Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        /// Logged-in students: fill the profile from their account and lock those fields.
        /// Guests: hide the personal fields, leaving only the grade.
        /// </summary>
        private void ApplySessionToProfile()
        {
            if (AppSession.IsInRole(Roles.Learner) && AppSession.UserID != null)
            {
                LearnerProfileInfo? info = new UserAccountDAL().GetLearnerProfile(AppSession.UserID.Value);
                if (info == null)
                    return;

                textFirstName.Text = info.FirstName;
                txtLastName.Text = info.LastName;
                txtEmail.Text = info.Email;
                txtSchoolName.Text = info.SchoolName;
                cmbProvince.Text = info.Province;

                foreach (Control field in new Control[] { textFirstName, txtLastName, txtEmail, txtSchoolName, cmbProvince })
                    field.Enabled = false;

                savedUserID = AppSession.UserID.Value;
            }
            else if (AppSession.IsInRole(Roles.Guest))
            {
                foreach (Control field in new Control[] {
                    txtFirstName, textFirstName, lblLastName, txtLastName, lblEmail, txtEmail,
                    lblSchoolName, txtSchoolName, lblProvince, cmbProvince })
                    field.Visible = false;

                // Move the grade up into the space the hidden fields leave behind.
                lblGrade.Location = txtFirstName.Location;
                cmbGrade.Location = textFirstName.Location;
            }
        }

        private void OpenSubjectsForm()
        {
            using (Form2 form2 = new Form2())
            {
                form2.ShowDialog(this);
            }
        }

        private bool IsValidEmail(string email)
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

        private void label2_Click(object? sender, EventArgs e)
        {
        }

        private void label4_Click(object? sender, EventArgs e)
        {
        }

        private void button3_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}