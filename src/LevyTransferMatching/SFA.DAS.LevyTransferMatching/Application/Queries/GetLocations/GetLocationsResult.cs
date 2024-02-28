using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations
{
    public class GetLocationsResult
    {
        public IEnumerable<GetLocationsListItem> Locations { get; set; }
    }
}
