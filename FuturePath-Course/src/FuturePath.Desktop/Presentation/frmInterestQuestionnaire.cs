using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmInterestQuestionnaire : frmAdvisoryStep
    {
        #region Constructors
        public frmInterestQuestionnaire()
        {
            InitializeComponent();
        }

        public frmInterestQuestionnaire(AppController app, frmMain main) : this()
        {
            this.app = app;
            this.main = main;
        }

        #endregion
        private List<QuestionRow> rows = new List<QuestionRow>();
        private bool loaded;
        public override void RefreshStep()
        {
            if (!loaded)
            {
                List<InterestQuestion> questions = app.Advising.Questions();
                int index = 0;
                foreach (InterestQuestion question in questions)
                {
                    QuestionRow row = new QuestionRow(question);
                    row.Dock = DockStyle.Fill;
                    foreach (InterestResponse answer in main.Input.Responses)
                    {
                        if (answer.QuestionId == question.Id)
                        {
                            row.Score = answer.Score;
                        }
                    }

                    row.ResponseChanged += ResponseChanged;
                    rows.Add(row);
                    pnlQuestions.Controls.Add(row, 0, index);
                    index++;
                }

                loaded = true;
            }

            ResponseChanged(this, EventArgs.Empty);
        }

        private void ResponseChanged(object? sender, EventArgs e)
        {
            bool complete = rows.Count > 0;
            foreach (QuestionRow row in rows)
            {
                if (row.Score == 0)
                {
                    complete = false;
                }
            }

            btnNext.Enabled = complete;
            lblMessage.ForeColor = complete ? Color.DarkGreen : Color.FromArgb(22, 45, 63);
            ShowMessage(complete ? "All questions answered." : "Answer every question before continuing.");
        }

        public override void SaveInput()
        {
            main.Input.Responses.Clear();
            foreach (QuestionRow row in rows)
            {
                if (row.Score == 0)
                {
                    continue;
                }

                InterestResponse response = new InterestResponse(row.QuestionId, row.Score);
                main.Input.Responses.Add(response);
            }

            AdvisingValidation.Interests(main.Input.Responses, app.Advising.Questions(), true);
        }
    }
}
