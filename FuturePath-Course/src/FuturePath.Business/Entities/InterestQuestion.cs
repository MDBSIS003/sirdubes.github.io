using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class InterestQuestion
    {
        private int id;
        private int categoryId;
        private string category = "";
        private string text = "";
        public int Id
        {
            get { return id; }

            set { id = value; }
        }

        public int CategoryId
        {
            get { return categoryId; }

            set { categoryId = value; }
        }

        public string Category
        {
            get { return category; }

            set { category = value; }
        }

        public string Text
        {
            get { return text; }

            set { text = value; }
        }
    }
}
