using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public sealed class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}
