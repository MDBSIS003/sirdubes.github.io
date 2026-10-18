using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class DegreeProgramme
    {
        private int id;
        private int facultyId;
        private string faculty = "";
        private string name = "";
        private string description = "";
        private bool active;
        private decimal minimumAverage;
        private List<ProgrammeRequirement> requirements = new List<ProgrammeRequirement>();
        private List<InterestWeight> interests = new List<InterestWeight>();
        public int Id
        {
            get { return id; }

            set { id = value; }
        }

        public int FacultyId
        {
            get { return facultyId; }

            set { facultyId = value; }
        }

        public string Faculty
        {
            get { return faculty; }

            set { faculty = value; }
        }

        public string Name
        {
            get { return name; }

            set { name = value; }
        }

        public string Description
        {
            get { return description; }

            set { description = value; }
        }

        public bool Active
        {
            get { return active; }

            set { active = value; }
        }

        public decimal MinimumAverage
        {
            get { return minimumAverage; }

            set { minimumAverage = value; }
        }

        public List<ProgrammeRequirement> Requirements
        {
            get { return requirements; }
        }

        public List<InterestWeight> Interests
        {
            get { return interests; }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
