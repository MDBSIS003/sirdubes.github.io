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
            WrapDisplayLabel(lblOutcomeDetails);
            WrapDisplayLabel(lblOutcomeHeading);
        }

        public frmAdvisoryResults(AppController app, frmMain main) : this()
        {
            this.app = app;
            this.main = main;
        }

        #endregion
        private string SpacedExplanation(string explanation)
        {
            return explanation.Replace("\r\n", "\n").Replace("\n", Environment.NewLine + Environment.NewLine).Trim();
        }

        public override void RefreshStep()
        {
            if (main.Preview == null)
            {
                return;
            }

            Recommendation result = main.Preview.Selected;
            lblOutcomeHeading.Text = result.OutcomeText + " - " + result.Programme;
            switch (result.Outcome)
            {
                case AdvisoryOutcome.StrongMatch:
                    lblOutcomeHeading.ForeColor = Color.DarkGreen;
                    break;
                case AdvisoryOutcome.NotCurrentlyEligible:
                    lblOutcomeHeading.ForeColor = Color.Firebrick;
                    break;
                default:
                    lblOutcomeHeading.ForeColor = Color.DarkGoldenrod;
                    break;
            }

            lblOutcomeDetails.Text = "Recommendation score: " + result.OverallScore.ToString("0.##") + "%" + Environment.NewLine +
                "Subject coverage: " + result.SubjectScore.ToString("0.##") + "%   |   Mark match: " + result.MarkScore.ToString("0.##") + "%" + Environment.NewLine +
                "Interest alignment: " + result.InterestScore.ToString("0.##") + "%" + Environment.NewLine + Environment.NewLine +
                SpacedExplanation(result.Explanation);
            foreach (Control card in new List<Control>(pnlAlternatives.Controls.Cast<Control>()))
            {
                card.Dispose();
            }
            pnlAlternatives.Controls.Clear();
            pnlAlternatives.RowStyles.Clear();
            pnlAlternatives.RowCount = 0;
            int rank = 0;
            foreach (Recommendation alternative in main.Preview.Alternatives)
            {
                rank++;
                TableLayoutPanel card = new TableLayoutPanel();
                card.AutoSize = true;
                card.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                card.ColumnCount = 1;
                card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                card.Dock = DockStyle.Top;
                card.Padding = new Padding(14);
                card.Margin = new Padding(0, 4, 0, 14);
                card.BackColor = Color.White;
                Label heading = new Label();
                heading.AutoSize = true;
                heading.Dock = DockStyle.Fill;
                heading.Font = lblOutcomeHeading.Font;
                heading.Text = rank + ". " + alternative.Programme;
                heading.Margin = new Padding(0, 0, 0, 10);
                Label details = new Label();
                details.AutoSize = true;
                details.Dock = DockStyle.Fill;
                details.Margin = new Padding(0);
                details.Text = alternative.OutcomeText + "   |   Score: " + alternative.OverallScore.ToString("0.##") + "%" +
                    Environment.NewLine + Environment.NewLine + SpacedExplanation(alternative.Explanation);
                card.Controls.Add(heading, 0, 0);
                card.Controls.Add(details, 0, 1);
                card.RowCount = 2;
                pnlAlternatives.RowCount = rank;
                pnlAlternatives.Controls.Add(card, 0, rank - 1);
                WrapDisplayLabel(heading);
                WrapDisplayLabel(details);
            }

            if (main.Preview.Alternatives.Count == 0)
            {
                Label empty = new Label();
                empty.AutoSize = true;
                empty.Dock = DockStyle.Fill;
                empty.Text = "No eligible or near-match alternatives can be confirmed. Discuss the missing information or gaps with an advisor.";
                empty.Padding = new Padding(14);
                empty.BackColor = Color.White;
                pnlAlternatives.Controls.Add(empty, 0, 0);
                pnlAlternatives.RowCount = 1;
                WrapDisplayLabel(empty);
            }

            lblMessage.ForeColor = Color.FromArgb(22, 45, 63);
            ShowMessage(RecommendationEngine.Disclaimer);
        }
    }
}
