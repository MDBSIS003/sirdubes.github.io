using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class SystemClock : IClock
    {
        public DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        public DateOnly Today
        {
            get
            {
                return DateOnly.FromDateTime(DateTime.Today);
            }
        }
    }
}
