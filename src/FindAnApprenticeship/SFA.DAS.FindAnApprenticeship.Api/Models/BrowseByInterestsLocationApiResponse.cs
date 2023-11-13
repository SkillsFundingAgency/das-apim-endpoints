using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterestsLocation;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class BrowseByInterestsLocationApiResponse
    {
        public SearchLocationResponseItem Location { get; set; }

        public static implicit operator BrowseByInterestsLocationApiResponse(BrowseByInterestsLocationQueryResult source)
        {
            if (source.LocationItem == null)
            {
                return new BrowseByInterestsLocationApiResponse
                {
                    Location = null
                };
            }
            return new BrowseByInterestsLocationApiResponse
            {
                Location = new SearchLocationResponseItem
                {
                    Lat = source.LocationItem.GeoPoint.First(),
                    Lon = source.LocationItem.GeoPoint.Last(),
                    LocationName = source.LocationItem.Name
                }
            };
        }
    }

    public class SearchLocationResponseItem
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string LocationName { get; set; }
    }
}