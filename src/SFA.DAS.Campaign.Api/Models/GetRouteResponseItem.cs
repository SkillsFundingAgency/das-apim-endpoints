using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Campaign.Api.Models
{
    public class GetRouteResponseItem
    {
        public string Route { get; set; }
        
        public static implicit operator GetRouteResponseItem (GetRoutesListItem source)
        {
            return new GetRouteResponseItem
            {
                Route = source.Name
            };
        }
    }
}