using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application
{
    public abstract class MediatorResultBase
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}
