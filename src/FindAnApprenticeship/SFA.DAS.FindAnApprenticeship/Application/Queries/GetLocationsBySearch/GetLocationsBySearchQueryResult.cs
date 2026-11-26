using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocationsBySearch
{
    public class GetLocationsBySearchQueryResult
    {
        public IEnumerable<GetLocationsListItem> Locations { get; set; }
    }
}
