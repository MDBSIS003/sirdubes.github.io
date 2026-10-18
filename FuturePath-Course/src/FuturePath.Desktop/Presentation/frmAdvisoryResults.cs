using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmAdvisoryResults : frmAdvisoryStep
    {
        #region Constructors
        public frmAdvisoryResults()
        {
            InitializeComponent();
        }

        public frmAdvisoryResults(AppController app, frmMain main) : this()
        {
            this.app = app;
            this.main = main;
        }

        #endregion
        public override void RefreshStep()
        {
            if (main.Preview == null)
            {
                return;
            }

            Recommendation result = main.Preview.Selected;
            lbltxtOutcome.Text = result.OutcomeText + " - " + result.Programme;
            lbltxtOutcome.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            switch (result.Outcome)
            {
                case AdvisoryOutcome.StrongMatch:
                    lbltxtOutcome.ForeColor = Color.DarkGreen;
                    break;
                case AdvisoryOutcome.NotCurrentlyEligible:
                    lbltxtOutcome.ForeColor = Color.Firebrick;
                    break;
                default:
                    lbltxtOutcome.ForeColor = Color.DarkGoldenrod;
                    break;
            }

            txtOutcome.Text = result.Programme + Environment.NewLine + result.OutcomeText + Environment.NewLine + "Score: " + result.OverallScore + "%" + Environment.NewLine + result.Explanation;
            txtAlternatives.Clear();
            foreach (Recommendation alternative in main.Preview.Alternatives)
            {
                txtAlternatives.AppendText(alternative.Programme + " - " + alternative.OutcomeText + " - " + alternative.OverallScore + "%" + Environment.NewLine);
                txtAlternatives.AppendText(alternative.Explanation + Environment.NewLine + Environment.NewLine);
            }

            if (main.Preview.Alternatives.Count == 0)
            {
                txtAlternatives.Text = "No eligible or near-match alternatives can be confirmed. Discuss the missing information or gaps with an advisor.";
            }

            ShowMessage(main.Input.Learner.Grade == 11 ? "Grade 11 results are provisional. " + RecommendationEngine.Disclaimer : RecommendationEngine.Disclaimer);
        }
    }
}
