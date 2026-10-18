using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public enum FieldKind
    {
        Text,
        LongText,
        Integer,
        Decimal,
        Boolean,
        Date,
        Lookup,
        Choice
    }
}
