using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FuturePath.Business;

namespace FuturePath.Desktop
{
    public partial class frmDegreeEligibility : frmAdvisoryStep
    {
        #region Constructors
        public frmDegreeEligibility()
        {
            InitializeComponent();
            WrapDisplayLabel(lblRequirements);
        }

        public frmDegreeEligibility(AppController app, frmMain main) : this()
        {
            this.app = app;
            this.main = main;
        }

        #endregion
        private List<DegreeProgramme> programmes = new List<DegreeProgramme>();
        private bool loaded;
        public override void RefreshStep()
        {
            if (loaded)
            {
                return;
            }

            programmes = app.Advising.Programmes();
            cboFaculty.DisplayMember = "Name";
            cboFaculty.ValueMember = "Id";
            cboFaculty.DataSource = app.Advising.Faculties();
            if (main.Input.PreferredFacultyId > 0)
            {
                cboFaculty.SelectedValue = main.Input.PreferredFacultyId;
            }

            foreach (DegreeProgramme programme in cboDegree.Items)
            {
                if (programme.Id == main.Input.DegreeId)
                {
                    cboDegree.SelectedItem = programme;
                    break;
                }
            }

            loaded = true;
        }

        private void cboFaculty_SelectedIndexChanged(object? sender, EventArgs e)
        {
            List<DegreeProgramme> choices = new List<DegreeProgramme>();
            if (cboFaculty.SelectedItem is LookupItem faculty)
            {
                foreach (DegreeProgramme programme in programmes)
                {
                    if (programme.FacultyId == faculty.Id)
                    {
                        choices.Add(programme);
                    }
                }
            }

            cboDegree.DisplayMember = "Name";
            cboDegree.ValueMember = "Id";
            cboDegree.DataSource = choices;
            cboDegree_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void cboDegree_SelectedIndexChanged(object? sender, EventArgs e)
        {
            btnNext.Enabled = cboDegree.SelectedItem != null;
            lblRequirements.Text = "";
            if (cboDegree.SelectedItem is not DegreeProgramme programme)
            {
                return;
            }

            System.Text.StringBuilder details = new System.Text.StringBuilder();
            details.AppendLine(programme.Description);
            details.AppendLine();
            details.AppendLine("Minimum overall average: " + programme.MinimumAverage.ToString("0.##") + "%");
            details.AppendLine();
            details.AppendLine("Subject requirements");
            foreach (ProgrammeRequirement requirement in programme.Requirements)
            {
                string group = requirement.AlternativeGroup == 0 ? "" : " (OR group " + requirement.AlternativeGroup + ")";
                string status = requirement.Mandatory ? "Required" : "Helpful, not compulsory";
                details.AppendLine("• " + requirement.Subject + ": " + requirement.MinimumMark.ToString("0.##") + "% — " + status + group);
                if (!string.IsNullOrWhiteSpace(requirement.Note))
                {
                    details.AppendLine("  " + requirement.Note);
                }
            }

            lblRequirements.Text = details.ToString().TrimEnd();
        }

        public override void SaveInput()
        {
            if (cboFaculty.SelectedItem is not LookupItem faculty || cboDegree.SelectedItem is not DegreeProgramme programme)
            {
                throw new ValidationException("Choose a faculty and degree.");
            }

            main.Input.PreferredFacultyId = faculty.Id;
            main.Input.DegreeId = programme.Id;
            main.Preview = app.Advising.Preview(main.Input);
        }
    }
}
