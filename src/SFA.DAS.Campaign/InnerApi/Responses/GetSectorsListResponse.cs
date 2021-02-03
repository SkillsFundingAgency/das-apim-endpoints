using System.Collections.Generic;

namespace SFA.DAS.Campaign.InnerApi.Responses
{
    public class GetSectorsListResponse
    {
        public IEnumerable<GetSectorsListItem> Sectors { get; set; }
    }
}