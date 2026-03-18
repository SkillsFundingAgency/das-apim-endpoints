using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations
{
    public class GetLocationsResult
    {
        public IEnumerable<GetLocationsListItem> Locations { get; set; }
    }
}
