using System.Collections.Generic;

namespace SFA.DAS.Campaign.InnerApi.Responses
{
    public class GetRoutesListResponse
    {
        public IEnumerable<GetRoutesListItem> Routes { get; set; }
    }
}