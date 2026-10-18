using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public class ReferenceDefinition
    {
        private string key;
        private string label;
        private ReferenceField[] fields;
        public string Key
        {
            get { return key; }
        }

        public string Label
        {
            get { return label; }
        }

        public ReferenceField[] Fields
        {
            get { return fields; }
        }

        public ReferenceDefinition(string key, string label, ReferenceField[] fields)
        {
            this.key = key;
            this.label = label;
            this.fields = fields;
        }
    }
}
