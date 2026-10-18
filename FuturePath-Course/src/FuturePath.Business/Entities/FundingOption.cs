using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class FundingOption
    {
        private int id;
        private string name = "";
        private string type = "";
        private string eligibility = "";
        private string documents = "";
        private DateOnly closingDate;
        private string contact = "";
        private bool sample;
        public int Id
        {
            get { return id; }

            set { id = value; }
        }

        public string Name
        {
            get { return name; }

            set { name = value; }
        }

        public string Type
        {
            get { return type; }

            set { type = value; }
        }

        public string Eligibility
        {
            get { return eligibility; }

            set { eligibility = value; }
        }

        public string Documents
        {
            get { return documents; }

            set { documents = value; }
        }

        public DateOnly ClosingDate
        {
            get { return closingDate; }

            set { closingDate = value; }
        }

        public string Contact
        {
            get { return contact; }

            set { contact = value; }
        }

        public bool Sample
        {
            get { return sample; }

            set { sample = value; }
        }
    }
}
