using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocationsBySearch
{
    public class GetLocationsBySearchQueryResult
    {
        public IEnumerable<GetLocationsListItem> Locations { get; set; }
    }
}
