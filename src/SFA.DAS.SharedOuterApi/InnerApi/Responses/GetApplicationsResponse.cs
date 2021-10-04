using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetApplicationsResponse
    {
        public IEnumerable<Models.Application> Applications { get; set; }
    }
}
