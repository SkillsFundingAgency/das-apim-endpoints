using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests
{
    public class GetApplicationsResponse
    {
        public IEnumerable<Models.Application> Applications { get; set; }
    }
}
