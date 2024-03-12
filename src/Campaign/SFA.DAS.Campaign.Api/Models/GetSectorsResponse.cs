using System.Collections.Generic;

namespace SFA.DAS.Campaign.Api.Models
{

    public class GetSectorsResponse
    {
        public IEnumerable<GetRouteResponseItem> Sectors { get; set; }
    }
}