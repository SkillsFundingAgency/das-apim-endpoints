using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetApplicationsResponse
    {
        public Standard Standard { get; set; }
        public IEnumerable<LevyTransferMatching.Models.Application> Applications { get; set; }
    }
}
