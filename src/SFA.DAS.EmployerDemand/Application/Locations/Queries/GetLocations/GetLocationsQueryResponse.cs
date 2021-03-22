using System.Collections.Generic;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Application.Locations.Queries.GetLocations
{
    public class GetLocationsQueryResponse
    {
        public IEnumerable<GetLocationsListItem> Locations { get ; set ; }
    }
}