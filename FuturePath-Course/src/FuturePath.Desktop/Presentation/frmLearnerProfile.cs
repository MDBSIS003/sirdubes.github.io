using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmLearnerProfile : frmAdvisoryStep
    {
        #region Constructors
        public frmLearnerProfile()
        {
            InitializeComponent();
            cboProvince.Items.AddRange(ReferenceCatalog.Provinces);
        }

        public frmLearnerProfile(AppController app, frmMain main) : this()
        {
            this.app = app;
            this.main = main;
        }

        #endregion
        private bool loaded;
        public override void RefreshStep()
        {
            if (loaded)
            {
                return;
            }

            LearnerProfile learner = main.Input.Learner;
            txtName.Text = learner.Name;
            txtSchool.Text = learner.School;
            txtEmail.Text = learner.Email;
            txtPhone.Text = learner.Phone;
            if (learner.Province.Length > 0)
            {
                cboProvince.SelectedItem = learner.Province;
            }

            if (learner.Grade == 11 || learner.Grade == 12)
            {
                cboGrade.SelectedItem = learner.Grade.ToString();
            }

            bool followUp = learner.Id > 0;
            txtName.ReadOnly = followUp && learner.Name.Trim().Length > 0;
            txtSchool.ReadOnly = followUp && learner.School.Trim().Length > 0;
            txtEmail.ReadOnly = followUp && learner.Email.Trim().Length > 0;
            txtPhone.ReadOnly = followUp && learner.Phone.Trim().Length > 0;
            cboProvince.Enabled = !followUp || learner.Province.Trim().Length == 0;
            txtName.Validating += Contact_Validating;
            txtEmail.Validating += Contact_Validating;
            txtPhone.Validating += Contact_Validating;
            txtName.TextChanged += ProfileChanged;
            txtEmail.TextChanged += ProfileChanged;
            txtPhone.TextChanged += ProfileChanged;
            txtSchool.Validating += Contact_Validating;
            cboProvince.Validating += Contact_Validating;
            cboGrade.Validating += Contact_Validating;
            loaded = true;
            ProfileChanged(this, EventArgs.Empty);
        }

        private void ProfileChanged(object? sender, EventArgs e)
        {
            if (!loaded)
            {
                return;
            }

            ReadProfile();
            try
            {
                ValidateProfileInput();
                btnNext.Enabled = true;
            }
            catch (ValidationException)
            {
                btnNext.Enabled = false;
            }
        }

        private void Contact_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                ReadProfile();
                ValidateProfileInput();
                ShowMessage("");
                errProfile.Clear();
            }
            catch (ValidationException ex)
            {
                ShowMessage(ex.Message);
                if (sender is Control control)
                {
                    errProfile.SetError(control, ex.Message);
                }
            }
        }

        private void ValidateProfileInput()
        {

            AdvisingValidation.ProfileCapture(main.Input.Learner);
        }

        private void ReadProfile()
        {
            LearnerProfile learner = main.Input.Learner;
            learner.Name = txtName.Text;
            learner.School = txtSchool.Text;
            learner.Province = cboProvince.Text;
            learner.Grade = cboGrade.SelectedIndex < 0 ? 0 : Convert.ToInt32(cboGrade.Text);
            learner.Email = txtEmail.Text;
            learner.Phone = txtPhone.Text;
        }

        public override void SaveInput()
        {
            ReadProfile();
            ValidateProfileInput();
            ShowMessage("");
        }
    }
}
