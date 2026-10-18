using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class AccessDeniedException : Exception
    {
        public AccessDeniedException() : base("Please sign in with an account authorised for this action.")
        {
        }
    }
}
