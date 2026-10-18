using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmFundingOptions : frmAdvisoryStep
    {
        #region Constructors
        public frmFundingOptions()
        {
            InitializeComponent();
            WrapDisplayLabel(lblFunding);
        }

        public frmFundingOptions(AppController app, frmMain main) : this()
        {
            this.app = app;
            this.main = main;
        }

        #endregion
        public override void RefreshStep()
        {
            chkFunding.Checked = main.Input.FundingRequested;
            btnNext.Text = "Complete and save";
            lblMessage.ForeColor = Color.FromArgb(22, 45, 63);
            ShowMessage("Review any funding options, then select Complete and save to finish your assessment. Sample funding information must be verified with the provider.");
            chkFunding_CheckedChanged(this, EventArgs.Empty);
        }

        private void chkFunding_CheckedChanged(object? sender, EventArgs e)
        {
            if (app == null)
            {
                return;
            }

            System.Text.StringBuilder details = new System.Text.StringBuilder();
            if (!chkFunding.Checked)
            {
                lblFunding.Text = "Funding guidance is optional. You can save your assessment without it.";
                return;
            }

            List<FundingOption> options = app.Advising.Funding(main.Input.DegreeId);
            foreach (FundingOption option in options)
            {
                details.Append(option.Name + " - " + option.Type + Environment.NewLine);
                details.Append("Eligibility: " + option.Eligibility + Environment.NewLine);
                details.Append("Documents: " + option.Documents + Environment.NewLine);
                details.Append("Closing date: " + option.ClosingDate.ToString("yyyy-MM-dd") + Environment.NewLine);
                details.Append("Contact / application: " + option.Contact + Environment.NewLine + Environment.NewLine + Environment.NewLine);
            }

            lblFunding.Text = details.ToString().TrimEnd();
            if (options.Count == 0)
            {
                lblFunding.Text = "No open, active funding records are linked to this programme or faculty. This does not establish funding ineligibility.";
            }
        }

        public override void SaveInput()
        {
            main.Input.FundingRequested = chkFunding.Checked;

            if (main.SavedSession == null)
            {
                main.SavedSession = app.Advising.Assess(main.Input);
            }
        }
    }
}
