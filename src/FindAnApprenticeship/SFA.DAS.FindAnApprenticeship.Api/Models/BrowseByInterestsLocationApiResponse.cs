using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterestsLocation;

namespace SFA.DAS.FindAnApprenticeship.Api.Models
{
    public class BrowseByInterestsLocationApiResponse
    {
        public SearchLocationApiResponse LocationApi { get; set; }

        public static implicit operator BrowseByInterestsLocationApiResponse(BrowseByInterestsLocationQueryResult source)
        {
            if (source.LocationItem == null)
            {
                return new BrowseByInterestsLocationApiResponse
                {
                    LocationApi = null
                };
            }
            return new BrowseByInterestsLocationApiResponse
            {
                LocationApi = source.LocationItem
            };
        }
    }
}