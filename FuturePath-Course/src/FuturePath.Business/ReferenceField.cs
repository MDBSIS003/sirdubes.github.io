using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{

    public class ReferenceField
    {
        public string Name { get; private set; }
        public string Label { get; private set; }
        public FieldKind Kind { get; private set; }
        public bool Required { get; private set; }
        public string? Lookup { get; private set; }
        public string[]? Choices { get; private set; }
        public decimal Minimum { get; private set; }
        public decimal Maximum { get; private set; }
        public int MaximumLength { get; private set; }

        public ReferenceField(string Name, string Label, FieldKind Kind = FieldKind.Text, bool Required = true, string? Lookup = null, string[]? Choices = null, decimal Minimum = 0, decimal Maximum = 100, int MaximumLength = 0)
        {
            this.Name = Name;
            this.Label = Label;
            this.Kind = Kind;
            this.Required = Required;
            this.Lookup = Lookup;
            this.Choices = Choices;
            this.Minimum = Minimum;
            this.Maximum = Maximum;
            this.MaximumLength = MaximumLength;
        }
    }
}
