using System.Collections.Generic;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetApplicationsResponse
    {
        public GetStandardsListItem Standard { get; set; }
        public IEnumerable<LevyTransferMatching.Models.Application> Applications { get; set; }
    }
}
