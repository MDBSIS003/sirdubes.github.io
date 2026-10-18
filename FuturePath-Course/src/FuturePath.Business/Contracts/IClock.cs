using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public interface IClock
    {
        DateTime UtcNow { get; }

        DateOnly Today { get; }
    }
}
