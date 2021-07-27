using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}