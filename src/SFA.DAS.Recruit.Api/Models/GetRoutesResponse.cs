using System.Collections.Generic;

namespace SFA.DAS.Recruit.Api.Models
{

    public class GetRoutesResponse
    {
        public IEnumerable<GetRouteResponseItem> Routes { get; set; }
    }
}