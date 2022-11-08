using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAvailableProviderLocations
{
    public class GetAvailableProviderLocationsQueryResult
    {
        public List<ProviderLocationModel> AvailableProviderLocations { get; set; } = new List<ProviderLocationModel>();
    }
}
