using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class ProgrammeRequirement
    {
        private int subjectId;
        private string subject = "";
        private decimal minimumMark;
        private int alternativeGroup;
        private bool mandatory = true;
        private string note = "";
        public int SubjectId
        {
            get { return subjectId; }

            set { subjectId = value; }
        }

        public string Subject
        {
            get { return subject; }

            set { subject = value; }
        }

        public decimal MinimumMark
        {
            get { return minimumMark; }

            set { minimumMark = value; }
        }

        // Zero means an independent requirement; positive groups are OR choices.
        public int AlternativeGroup
        {
            get { return alternativeGroup; }

            set { alternativeGroup = value; }
        }

        public bool Mandatory
        {
            get { return mandatory; }

            set { mandatory = value; }
        }

        public string Note
        {
            get { return note; }

            set { note = value; }
        }
    }
}
