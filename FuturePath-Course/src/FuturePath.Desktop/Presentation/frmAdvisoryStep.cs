using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmAdvisoryStep : Form
    {
        #region Data Members
        protected AppController app = null!;
        protected frmMain main = null!;
        #endregion
        public frmAdvisoryStep()
        {
            InitializeComponent();
        }

        protected void Configure(string title, int step)
        {
            lblTitle.Text = title;
            lblStep.Text = "Step " + step + " of 7";
            btnBack.Enabled = step > 1 && step < 7;
            if (step == 7)
            {
                btnNext.Text = "Finish";
            }
        }


        public virtual void RefreshStep()
        {
        }

        public virtual void SaveInput()
        {
        }

        protected void ShowMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = message.Length > 0;
        }

        protected void WrapDisplayLabel(Label label)
        {
            label.AutoSize = true;
            label.UseMnemonic = false;
            Control parent = label.Parent!;
            EventHandler resize = (sender, e) =>
            {
                int width = Math.Max(1, parent.ClientSize.Width - parent.Padding.Horizontal - label.Margin.Horizontal);
                if (label.MaximumSize.Width != width)
                {
                    label.MaximumSize = new Size(width, 0);
                }
            };
            parent.SizeChanged += resize;
            label.Disposed += (sender, e) => parent.SizeChanged -= resize;
            resize(parent, EventArgs.Empty);
        }

        private void btnBack_Click(object? sender, EventArgs e)
        {
            main.GoBack();
        }

        private void btnNext_Click(object? sender, EventArgs e)
        {
            main.GoNext();
        }

        private void btnEnd_Click(object? sender, EventArgs e)
        {
            main.EndSession();
        }
    }
}
