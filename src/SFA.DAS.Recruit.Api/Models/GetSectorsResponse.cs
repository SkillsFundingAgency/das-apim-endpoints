using System.Collections.Generic;

namespace SFA.DAS.Recruit.Api.Models
{

    public class GetSectorsResponse
    {
        public IEnumerable<GetRouteResponseItem> Sectors { get; set; }
    }
}