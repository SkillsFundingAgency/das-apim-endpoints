using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAllProviderLocations
{
    public class GetAllProviderLocationsQueryResult
    {
        public List<ProviderLocationModel> ProviderLocations { get; set; }
    }
}
