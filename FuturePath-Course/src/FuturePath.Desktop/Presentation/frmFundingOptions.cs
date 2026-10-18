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
            btnNext.Text = "Save and view summary";
            chkFunding_CheckedChanged(this, EventArgs.Empty);
        }

        private void chkFunding_CheckedChanged(object? sender, EventArgs e)
        {
            if (app == null)
            {
                return;
            }

            txtFunding.Clear();
            if (!chkFunding.Checked)
            {
                txtFunding.Text = "Funding guidance is optional. You can save your assessment without it.";
                return;
            }

            List<FundingOption> options = app.Advising.Funding(main.Input.DegreeId);
            foreach (FundingOption option in options)
            {
                txtFunding.AppendText(option.Name + " - " + option.Type + Environment.NewLine);
                txtFunding.AppendText("Eligibility: " + option.Eligibility + Environment.NewLine);
                txtFunding.AppendText("Documents: " + option.Documents + Environment.NewLine);
                txtFunding.AppendText("Closing date: " + option.ClosingDate.ToString("yyyy-MM-dd") + Environment.NewLine);
                txtFunding.AppendText("Contact / application: " + option.Contact + Environment.NewLine + "Sample information - verify with provider." + Environment.NewLine + Environment.NewLine);
            }

            if (options.Count == 0)
            {
                txtFunding.Text = "No open, active funding records are linked to this programme or faculty. This does not establish funding ineligibility.";
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
