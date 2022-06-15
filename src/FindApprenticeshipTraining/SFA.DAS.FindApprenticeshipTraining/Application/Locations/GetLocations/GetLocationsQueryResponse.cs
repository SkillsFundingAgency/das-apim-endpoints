using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Locations.GetLocations
{
    public class GetLocationsQueryResponse
    {
        public IEnumerable<GetLocationsListItem> Locations { get ; set ; }
    }
}