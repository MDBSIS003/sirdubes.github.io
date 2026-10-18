using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class SessionSummary
    {
        private int degreeId;
        private int facultyId;
        public int DegreeId
        {
            get { return degreeId; }

            set { degreeId = value; }
        }

        public int FacultyId
        {
            get { return facultyId; }

            set { facultyId = value; }
        }

        private int id;
        private int learnerId;
        private string learner = "";
        private int grade;
        private DateTime createdUtc;
        private string programme = "";
        private string faculty = "";
        private string outcome = "";
        private decimal score;
        private bool fundingRequested;
        private string explanation = "";
        public int Id
        {
            get { return id; }

            set { id = value; }
        }

        public int LearnerId
        {
            get { return learnerId; }

            set { learnerId = value; }
        }

        public string Learner
        {
            get { return learner; }

            set { learner = value; }
        }

        public int Grade
        {
            get { return grade; }

            set { grade = value; }
        }

        public DateTime CreatedUtc
        {
            get { return createdUtc; }

            set { createdUtc = value; }
        }

        public string Programme
        {
            get { return programme; }

            set { programme = value; }
        }

        public string Faculty
        {
            get { return faculty; }

            set { faculty = value; }
        }

        public string Outcome
        {
            get { return outcome; }

            set { outcome = value; }
        }

        public decimal Score
        {
            get { return score; }

            set { score = value; }
        }

        public bool FundingRequested
        {
            get { return fundingRequested; }

            set { fundingRequested = value; }
        }

        public string Explanation
        {
            get { return explanation; }

            set { explanation = value; }
        }
    }
}
