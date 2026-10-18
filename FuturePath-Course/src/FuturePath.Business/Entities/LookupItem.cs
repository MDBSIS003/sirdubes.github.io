using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class LookupItem
    {
        private int id;
        private string name = "";
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

        public override string ToString()
        {
            return Name;
        }
    }
}
