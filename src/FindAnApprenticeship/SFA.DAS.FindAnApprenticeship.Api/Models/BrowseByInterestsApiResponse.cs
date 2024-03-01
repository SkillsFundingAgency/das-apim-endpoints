using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class BrowseByInterestsApiResponse
    {
        public List<RouteApiResponse> Routes { get; set; }
        public static implicit operator BrowseByInterestsApiResponse(BrowseByInterestsResult source)
        {
            return new BrowseByInterestsApiResponse
            {
                Routes = source.Routes.Select(r =>(RouteApiResponse)r).ToList()
            };
        }

        

    }
}