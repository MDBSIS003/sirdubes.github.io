using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class AdvisoryInput
    {
        private LearnerProfile learner = new LearnerProfile();
        private int degreeId;
        private int preferredFacultyId;
        private bool subjectsComplete;
        private bool fundingRequested;
        private List<SubjectMark> marks = new List<SubjectMark>();
        private List<InterestResponse> responses = new List<InterestResponse>();
        public LearnerProfile Learner
        {
            get { return learner; }

            set { learner = value; }
        }

        public int DegreeId
        {
            get { return degreeId; }

            set { degreeId = value; }
        }

        public int PreferredFacultyId
        {
            get { return preferredFacultyId; }

            set { preferredFacultyId = value; }
        }

        public bool SubjectsComplete
        {
            get { return subjectsComplete; }

            set { subjectsComplete = value; }
        }

        public bool FundingRequested
        {
            get { return fundingRequested; }

            set { fundingRequested = value; }
        }

        public List<SubjectMark> Marks
        {
            get { return marks; }

            set { marks = value; }
        }

        public List<InterestResponse> Responses
        {
            get { return responses; }

            set { responses = value; }
        }
    }
}
