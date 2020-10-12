using System.Collections.Generic;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}