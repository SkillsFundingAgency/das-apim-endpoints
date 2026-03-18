using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

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