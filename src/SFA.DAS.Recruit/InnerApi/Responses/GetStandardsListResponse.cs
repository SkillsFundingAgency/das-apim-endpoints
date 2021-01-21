using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}