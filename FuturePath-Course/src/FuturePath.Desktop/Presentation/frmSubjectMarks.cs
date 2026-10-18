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
            ShowMessage("An unconfirmed subject list produces More Information Needed.");
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

            btnNext.Enabled = main.Input.Marks.Count > 0;
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

            if (found == null)
            {
                found = new SubjectMark(subject.Id, subject.Name, nudMark.Value);
                main.Input.Marks.Add(found);
            }

            found.Mark = nudMark.Value;
            PopulateList();
            ShowMessage("Subject saved in this draft. The database is saved at the funding step.");
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

            main.Input.Marks.Remove((SubjectMark)lstMarks.SelectedItems[0].Tag!);
            PopulateList();
        }

        public override void SaveInput()
        {
            AdvisingValidation.Marks(main.Input.Marks, app.Advising.Subjects());
            main.Input.SubjectsComplete = chkComplete.Checked;
        }
    }
}
