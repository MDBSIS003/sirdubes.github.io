#nullable enable
using FuturePath.Business;

namespace FuturePath.Desktop
{
    partial class QuestionRow
    {
        private System.ComponentModel.IContainer? components;
        private TableLayoutPanel tblQuestion = null!;
        private Label lblQuestion = null!;
        private FlowLayoutPanel pnlAnswers = null!;
        private RadioButton rad1 = null!;
        private RadioButton rad2 = null!;
        private RadioButton rad3 = null!;
        private RadioButton rad4 = null!;
        private RadioButton rad5 = null!;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            tblQuestion = new TableLayoutPanel();
            tblQuestion.Name = "tblQuestion";
            lblQuestion = new Label();
            lblQuestion.Name = "lblQuestion";
            pnlAnswers = new FlowLayoutPanel();
            pnlAnswers.Name = "pnlAnswers";
            rad1 = new RadioButton();
            rad1.Name = "rad1";
            rad2 = new RadioButton();
            rad2.Name = "rad2";
            rad3 = new RadioButton();
            rad3.Name = "rad3";
            rad4 = new RadioButton();
            rad4.Name = "rad4";
            rad5 = new RadioButton();
            rad5.Name = "rad5";
            SuspendLayout();
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("Segoe UI", 10F);
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tblQuestion.AutoSize = true;
            tblQuestion.Dock = DockStyle.Top;
            tblQuestion.ColumnCount = 1;
            tblQuestion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            lblQuestion.AutoSize = true;
            lblQuestion.Dock = DockStyle.Fill;
            lblQuestion.Margin = new Padding(4, 12, 4, 8);
            pnlAnswers.AutoSize = true;
            pnlAnswers.Dock = DockStyle.Fill;
            pnlAnswers.WrapContents = true;
            tblQuestion.Controls.Add(lblQuestion, 0, 0);
            tblQuestion.Controls.Add(pnlAnswers, 0, 1);
            rad1.Text = "1 - Strongly disagree";
            rad1.AutoSize = true;
            rad1.Margin = new Padding(4, 4, 16, 8);
            rad1.CheckedChanged += AnswerChanged;
            pnlAnswers.Controls.Add(rad1);
            rad2.Text = "2 - Disagree";
            rad2.AutoSize = true;
            rad2.Margin = new Padding(4, 4, 16, 8);
            rad2.CheckedChanged += AnswerChanged;
            pnlAnswers.Controls.Add(rad2);
            rad3.Text = "3 - Neutral";
            rad3.AutoSize = true;
            rad3.Margin = new Padding(4, 4, 16, 8);
            rad3.CheckedChanged += AnswerChanged;
            pnlAnswers.Controls.Add(rad3);
            rad4.Text = "4 - Agree";
            rad4.AutoSize = true;
            rad4.Margin = new Padding(4, 4, 16, 8);
            rad4.CheckedChanged += AnswerChanged;
            pnlAnswers.Controls.Add(rad4);
            rad5.Text = "5 - Strongly agree";
            rad5.AutoSize = true;
            rad5.Margin = new Padding(4, 4, 16, 8);
            rad5.CheckedChanged += AnswerChanged;
            pnlAnswers.Controls.Add(rad5);
            Controls.Add(tblQuestion);
            Name = "QuestionRow";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
