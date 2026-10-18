using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class AdvisorySession
    {
        private int id;
        private DateTime createdUtc;
        private string ruleVersion = RecommendationEngine.RuleVersion;
        private AdvisoryInput input = new AdvisoryInput();
        private Recommendation selected = new Recommendation();
        private List<Recommendation> alternatives = new List<Recommendation>();
        private Dictionary<string, decimal> interestScores = new Dictionary<string, decimal>();
        public int Id
        {
            get { return id; }

            set { id = value; }
        }

        public DateTime CreatedUtc
        {
            get { return createdUtc; }

            set { createdUtc = value; }
        }

        public string RuleVersion
        {
            get { return ruleVersion; }

            set { ruleVersion = value; }
        }

        public AdvisoryInput Input
        {
            get { return input; }

            set { input = value; }
        }

        public Recommendation Selected
        {
            get { return selected; }

            set { selected = value; }
        }

        public List<Recommendation> Alternatives
        {
            get { return alternatives; }

            set { alternatives = value; }
        }

        public Dictionary<string, decimal> InterestScores
        {
            get { return interestScores; }

            set { interestScores = value; }
        }
    }
}
