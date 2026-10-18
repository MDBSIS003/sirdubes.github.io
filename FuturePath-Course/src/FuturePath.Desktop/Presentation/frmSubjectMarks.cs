using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmSubjectMarks : frmAdvisoryStep
    {
        #region Constructors
        public frmSubjectMarks()
        {
            InitializeComponent();
        }

        public frmSubjectMarks(AppController app, frmMain main) : this()
        {
            this.app = app;
            this.main = main;
        }

        #endregion
        private bool loaded;
        public override void RefreshStep()
        {
            if (!loaded)
            {
                cboSubject.DisplayMember = "Name";
                cboSubject.ValueMember = "Id";
                cboSubject.DataSource = app.Advising.Subjects();
                chkComplete.Checked = main.Input.SubjectsComplete;
                loaded = true;
            }

            PopulateList();
            UpdateProgress();
        }

        private void PopulateList()
        {
            lstMarks.Items.Clear();
            foreach (SubjectMark mark in main.Input.Marks)
            {
                ListViewItem item = new ListViewItem(mark.SubjectName);
                item.SubItems.Add(mark.Mark.ToString("0.##"));
                item.Tag = mark;
                lstMarks.Items.Add(item);
            }

            UpdateProgress();
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            if (cboSubject.SelectedItem is not LookupItem subject)
            {
                ShowMessage("Choose a subject.");
                return;
            }

            SubjectMark? found = null;
            foreach (SubjectMark mark in main.Input.Marks)
            {
                if (mark.SubjectId == subject.Id)
                {
                    found = mark;
                    break;
                }
            }

            if (found != null && (lstMarks.SelectedItems.Count == 0 ||
                ((SubjectMark)lstMarks.SelectedItems[0].Tag!).SubjectId != found.SubjectId))
            {
                ShowMessage("This subject is already captured. Select its row to update the mark.");
                return;
            }

            if (found == null)
            {
                found = new SubjectMark(subject.Id, subject.Name, nudMark.Value);
                main.Input.Marks.Add(found);
            }

            found.Mark = nudMark.Value;
            PopulateList();
            UpdateProgress();
        }

        private void lstMarks_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (lstMarks.SelectedItems.Count == 0)
            {
                return;
            }

            SubjectMark mark = (SubjectMark)lstMarks.SelectedItems[0].Tag!;
            cboSubject.SelectedValue = mark.SubjectId;
            nudMark.Value = mark.Mark;
        }

        private void btnRemove_Click(object? sender, EventArgs e)
        {
            if (lstMarks.SelectedItems.Count == 0)
            {
                return;
            }

            if (MessageBox.Show(this, "Remove the selected subject and mark?", "Future Path", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            main.Input.Marks.Remove((SubjectMark)lstMarks.SelectedItems[0].Tag!);
            PopulateList();
        }

        private void UpdateProgress()
        {
            bool enough = main.Input.Marks.Count >= AdvisingValidation.MinimumSubjects;
            btnNext.Enabled = enough && chkComplete.Checked;
            lblMessage.ForeColor = btnNext.Enabled ? Color.DarkGreen : Color.FromArgb(22, 45, 63);
            ShowMessage("Subjects entered: " + main.Input.Marks.Count + ". Enter at least seven different subjects and confirm the complete list.");
        }

        private void chkComplete_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateProgress();
        }

        public override void SaveInput()
        {
            AdvisingValidation.Marks(main.Input.Marks, app.Advising.Subjects());
            if (!chkComplete.Checked)
            {
                throw new ValidationException("Confirm that this is your complete subject list.");
            }

            main.Input.SubjectsComplete = true;
        }
    }
}
