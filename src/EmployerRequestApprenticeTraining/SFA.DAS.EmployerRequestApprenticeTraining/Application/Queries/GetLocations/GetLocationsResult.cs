using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations
{
    public class GetLocationsResult
    {
        public List<GetLocationsListItem> Locations { get; set; }
    }
}