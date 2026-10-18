using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmReports : frmStaffForm
    {
        public frmReports()
        {
            InitializeComponent();
        }

        private AppController app = null!;
        public frmReports(AppController app) : this()
        {
            this.app = app;
            app.Access.RequireStaff();
            dtpFrom.Value = new DateTime(2026, 1, 1);
            dtpTo.Value = DateTime.Today;
            List<LookupItem> faculties = app.Reports.Faculties();
            faculties.Insert(0, new LookupItem {
                Id = 0,
                Name = "All faculties"
            });
            cboFaculty.DisplayMember = "Name";
            cboFaculty.ValueMember = "Id";
            cboFaculty.DataSource = faculties;
            cboOutcome.Items.AddRange(new object[] {
                "All",
                "Strong Match",
                "Possible Match",
                "Not Currently Eligible",
                "More Information Needed"
            });
            cboOutcome.SelectedIndex = 0;
        }

        private void cboFaculty_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (app == null || cboFaculty.SelectedItem is not LookupItem faculty)
            {
                return;
            }

            List<DegreeProgramme> choices = new List<DegreeProgramme>();
            choices.Add(new DegreeProgramme {
                Id = 0,
                Name = "All degrees"
            });
            foreach (DegreeProgramme degree in app.Reports.Programmes())
            {
                if (faculty.Id == 0 || degree.FacultyId == faculty.Id)
                {
                    choices.Add(degree);
                }
            }

            cboDegree.DisplayMember = "Name";
            cboDegree.ValueMember = "Id";
            cboDegree.DataSource = choices;
        }

        private void Preview(bool exceptions)
        {
            try
            {
                DateOnly from = DateOnly.FromDateTime(dtpFrom.Value);
                DateOnly to = DateOnly.FromDateTime(dtpTo.Value);
                LookupItem faculty = (LookupItem)cboFaculty.SelectedItem!;
                DegreeProgramme degree = (DegreeProgramme)cboDegree.SelectedItem!;
                string outcome = cboOutcome.SelectedIndex == 0 ? "" : cboOutcome.Text;
                ReportDocument report = app.Reports.Activity(from, to, exceptions, txtSearch.Text, faculty.Id, degree.Id, outcome);
                using (frmReportPreview form = new frmReportPreview(app, report))
                {
                    form.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void btnActivity_Click(object? sender, EventArgs e)
        {
            Preview(false);
        }

        private void btnExceptions_Click(object? sender, EventArgs e)
        {
            Preview(true);
        }
    }
}
