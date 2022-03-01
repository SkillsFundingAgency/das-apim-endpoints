using System.Collections.Generic;
using SFA.DAS.Campaign.InnerApi.Responses;

namespace SFA.DAS.Campaign.Api.Models
{

    public class GetSectorsResponse
    {
        public IEnumerable<GetRouteResponseItem> Sectors { get; set; }
    }
}