using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetApplicationsResponse
    {
        public Standard Standard { get; set; }
        public IEnumerable<SharedOuterApi.Models.Application> Applications { get; set; }
    }
}
