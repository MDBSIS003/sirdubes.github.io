using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class SubjectMark
    {
        private int subjectId;
        private string subjectName = "";
        private decimal mark;
        #region Constructors
        public SubjectMark()
        {
        }

        public SubjectMark(int subjectId, string subjectName, decimal mark)
        {
            this.subjectId = subjectId;
            this.subjectName = subjectName;
            this.mark = mark;
        }

        #endregion
        public int SubjectId
        {
            get { return subjectId; }

            set { subjectId = value; }
        }

        public string SubjectName
        {
            get { return subjectName; }

            set { subjectName = value; }
        }

        public decimal Mark
        {
            get { return mark; }

            set { mark = value; }
        }
    }
}
