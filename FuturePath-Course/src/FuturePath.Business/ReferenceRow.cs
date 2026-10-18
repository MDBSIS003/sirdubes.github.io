using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class ReferenceRow
    {
        private int id;
        private Dictionary<string, object?> values = new Dictionary<string, object?>();
        public int Id
        {
            get { return id; }

            set { id = value; }
        }

        public Dictionary<string, object?> Values
        {
            get { return values; }

            set { values = value; }
        }

        public string Text(string name)
        {
            return Convert.ToString(Values.GetValueOrDefault(name), System.Globalization.CultureInfo.InvariantCulture) ?? "";
        }
    }
}
