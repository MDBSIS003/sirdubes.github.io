using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FuturePath.Business
{
    public enum AdvisoryOutcome
    {
        StrongMatch,
        PossibleMatch,
        NotCurrentlyEligible,
        MoreInformationNeeded
    }
}
