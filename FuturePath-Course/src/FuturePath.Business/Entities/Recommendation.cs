using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class Recommendation
    {
        private int degreeId;
        private string programme = "";
        private string faculty = "";
        private AdvisoryOutcome outcome;
        private decimal subjectScore;
        private decimal markScore;
        private decimal interestScore;
        private decimal preferenceScore;
        private decimal overallScore;
        private string explanation = "";
        public int DegreeId
        {
            get { return degreeId; }

            set { degreeId = value; }
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

        public AdvisoryOutcome Outcome
        {
            get { return outcome; }

            set { outcome = value; }
        }

        public decimal SubjectScore
        {
            get { return subjectScore; }

            set { subjectScore = value; }
        }

        public decimal MarkScore
        {
            get { return markScore; }

            set { markScore = value; }
        }

        public decimal InterestScore
        {
            get { return interestScore; }

            set { interestScore = value; }
        }

        public decimal PreferenceScore
        {
            get { return preferenceScore; }

            set { preferenceScore = value; }
        }

        public decimal OverallScore
        {
            get { return overallScore; }

            set { overallScore = value; }
        }

        public string Explanation
        {
            get { return explanation; }

            set { explanation = value; }
        }

        public string OutcomeText
        {
            get
            {
                switch (Outcome)
                {
                    case AdvisoryOutcome.StrongMatch:
                        return "Strong Match";
                    case AdvisoryOutcome.PossibleMatch:
                        return "Possible Match";
                    case AdvisoryOutcome.NotCurrentlyEligible:
                        return "Not Currently Eligible";
                    default:
                        return "More Information Needed";
                }
            }
        }
    }
}
