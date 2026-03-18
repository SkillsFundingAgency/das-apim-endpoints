using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations
{
    public class GetLocationsResult
    {
        public List<GetLocationsListItem> Locations { get; set; }
    }
}