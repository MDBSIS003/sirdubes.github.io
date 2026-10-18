using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class QuestionRow : UserControl
    {
        private int questionId;
        public event EventHandler? ResponseChanged;
        public int QuestionId
        {
            get { return questionId; }
        }

        public QuestionRow()
        {
            InitializeComponent();
        }

        public QuestionRow(InterestQuestion question) : this()
        {
            questionId = question.Id;
            lblQuestion.Text = question.Text;
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public int Score
        {
            get
            {
                if (rad1.Checked)
                {
                    return 1;
                }

                if (rad2.Checked)
                {
                    return 2;
                }

                if (rad3.Checked)
                {
                    return 3;
                }

                if (rad4.Checked)
                {
                    return 4;
                }

                if (rad5.Checked)
                {
                    return 5;
                }

                return 0;
            }

            set
            {
                rad1.Checked = value == 1;
                rad2.Checked = value == 2;
                rad3.Checked = value == 3;
                rad4.Checked = value == 4;
                rad5.Checked = value == 5;
            }
        }

        private void AnswerChanged(object? sender, EventArgs e)
        {
            if (ResponseChanged != null)
            {
                ResponseChanged(this, EventArgs.Empty);
            }
        }
    }
}
