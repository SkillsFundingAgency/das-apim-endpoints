using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetRouteResponseItem
    {
        public string Route { get; set; }
        public int Id { get; set; }
        
        public static implicit operator GetRouteResponseItem (GetRoutesListItem source)
        {
            return new GetRouteResponseItem
            {
                Route = source.Name,
                Id = source.Id
            };
        }

    }
}