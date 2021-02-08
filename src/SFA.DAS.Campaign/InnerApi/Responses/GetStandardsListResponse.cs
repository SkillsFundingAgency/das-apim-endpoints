using System.Collections.Generic;

namespace SFA.DAS.Campaign.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
        public int TotalFiltered { get ; set ; }
        public int Total { get ; set ; }
    }
}