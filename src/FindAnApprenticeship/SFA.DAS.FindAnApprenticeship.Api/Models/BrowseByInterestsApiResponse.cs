using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class BrowseByInterestsApiResponse
    {
        public List<RouteViewModel> Routes { get; set; }
        public static implicit operator BrowseByInterestsApiResponse(BrowseByInterestsResult source)
        {
            return new BrowseByInterestsApiResponse
            {
                Routes = source.Routes.Select(r => new RouteViewModel
                {
                    Name = r.Name,
                    Id = r.Id
                }).ToList()
            };
        }

        public class RouteViewModel
        {
             public string Name { get; set; }
             public int Id { get; set; }
        }

    }
}