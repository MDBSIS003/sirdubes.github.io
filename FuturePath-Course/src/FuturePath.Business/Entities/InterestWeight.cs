using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class InterestWeight
    {
        private int categoryId;
        private decimal weight;
        public int CategoryId
        {
            get { return categoryId; }

            set { categoryId = value; }
        }

        public decimal Weight
        {
            get { return weight; }

            set { weight = value; }
        }
    }
}
