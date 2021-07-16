using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{ 
    public class ApplicationDetailsResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
        public PledgeDto Opportunity { get; set; }
    }
}