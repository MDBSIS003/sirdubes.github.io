using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmMain : Form
    {
        #region Data Members
        private AppController app = null!;
        private frmWelcome? welcome;
        private frmAdvisoryStep? [] steps = new frmAdvisoryStep? [7];
        private int currentStep;
        private AdvisoryInput input = new AdvisoryInput();
        private AdvisorySession? preview;
        private AdvisorySession? savedSession;
        public AdvisoryInput Input
        {
            get { return input; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public AdvisorySession? Preview
        {
            get { return preview; }

            set { preview = value; }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public AdvisorySession? SavedSession
        {
            get { return savedSession; }

            set { savedSession = value; }
        }

        #endregion
        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(AppController app) : this()
        {
            this.app = app;
        }

        private async void frmMain_Shown(object? sender, EventArgs e)
        {
            if (app == null)
            {
                return;
            }

            UseWaitCursor = true;
            try
            {
                await System.Threading.Tasks.Task.Run(app.Store.Initialize);
                if (IsDisposed || Disposing) { return; }
            }
            catch (Exception ex)
            {
                if (IsDisposed || Disposing) { return; }
                UseWaitCursor = false;
                using (frmDatabaseSetup setup = new frmDatabaseSetup(app, ex.Message))
                {
                    if (setup.ShowDialog(this) != DialogResult.OK)
                    {
                        Close();
                        return;
                    }
                }
            }
            finally
            {
                if (!IsDisposed && !Disposing) { UseWaitCursor = false; }
            }

            ShowOverview();
        }

        public void ShowOverview()
        {
            CloseSteps();
            app.Access.ClearLearnerWorkspace();
            mnuStaff.Visible = app.Access.IsStaff;
            mnuAdmin.Visible = app.Access.Role == UserRole.Administrator;
            lblStatus.Text = app.Access.IsStaff ? app.Access.DisplayName + " | " + app.Access.Role : "Private learner workspace | No sign-in needed";
            if (welcome != null)
            {
                welcome.Dispose();
            }

            welcome = new frmWelcome(app, this);
            welcome.MdiParent = this;
            welcome.WindowState = FormWindowState.Maximized;
            welcome.Show();
        }

        public void StartAdvisory(AdvisoryInput? followUp = null)
        {
            CloseSteps();
            input = followUp ?? new AdvisoryInput();
            preview = null;
            savedSession = null;
            if (welcome != null)
            {
                welcome.Hide();
            }

            mnuStaff.Visible = false;
            ShowStep(0);
        }

        private void ShowStep(int index)
        {
            foreach (frmAdvisoryStep? step in steps)
            {
                if (step != null)
                {
                    step.Hide();
                }
            }

            if (steps[index] == null)
            {
                switch (index)
                {
                    case 0:
                        steps[index] = new frmLearnerProfile(app, this);
                        break;
                    case 1:
                        steps[index] = new frmSubjectMarks(app, this);
                        break;
                    case 2:
                        steps[index] = new frmInterestQuestionnaire(app, this);
                        break;
                    case 3:
                        steps[index] = new frmDegreeEligibility(app, this);
                        break;
                    case 4:
                        steps[index] = new frmAdvisoryResults(app, this);
                        break;
                    case 5:
                        steps[index] = new frmFundingOptions(app, this);
                        break;
                    case 6:
                        steps[index] = new frmAdvisorySummary(app, this);
                        break;
                }

                steps[index]!.MdiParent = this;
                steps[index]!.WindowState = FormWindowState.Maximized;
            }

            currentStep = index;
            steps[index]!.RefreshStep();
            steps[index]!.Show();
            steps[index]!.Activate();
        }

        public void GoNext()
        {
            try
            {
                if (currentStep == 6)
                {
                    ShowOverview();
                    return;
                }

                steps[currentStep]!.SaveInput();
                ShowStep(currentStep + 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Check your entries", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void GoBack()
        {
            if (currentStep > 0 && currentStep < 6)
            {
                ShowStep(currentStep - 1);
            }
        }

        public void EndSession()
        {
            if (MessageBox.Show(this, "End this learner workspace? Unsaved entries will be lost.", "Future Path", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ShowOverview();
            }
        }

        private void CloseSteps()
        {
            foreach (frmAdvisoryStep? step in steps)
            {
                if (step != null)
                {
                    step.Dispose();
                }
            }

            steps = new frmAdvisoryStep? [7];
            input = new AdvisoryInput();
            preview = null;
            savedSession = null;
        }

        private void frmMain_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (steps[0] != null && savedSession == null && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = MessageBox.Show(this, "Close Future Path and discard this draft?", "Future Path", MessageBoxButtons.YesNo) != DialogResult.Yes;
            }
        }

        private void mnuHome_Click(object? sender, EventArgs e)
        {
            if (steps[0] != null && savedSession == null)
            {
                EndSession();
            }
            else
            {
                ShowOverview();
            }
        }

        private void mnuSessions_Click(object? sender, EventArgs e)
        {
            using (frmLearnerSessions form = new frmLearnerSessions(app, this))
            {
                form.ShowDialog(this);
            }
        }

        private void mnuReports_Click(object? sender, EventArgs e)
        {
            using (frmReports form = new frmReports(app))
            {
                form.ShowDialog(this);
            }
        }

        private void mnuAdmin_Click(object? sender, EventArgs e)
        {
            using (frmAdminMaintenance form = new frmAdminMaintenance(app))
            {
                form.ShowDialog(this);
            }
        }

        private void mnuPassword_Click(object? sender, EventArgs e)
        {
            using (frmPassword form = new frmPassword(app))
            {
                form.ShowDialog(this);
            }
        }

        private void mnuLogout_Click(object? sender, EventArgs e)
        {
            app.Access.SignOut();
            ShowOverview();
        }
    }
}
