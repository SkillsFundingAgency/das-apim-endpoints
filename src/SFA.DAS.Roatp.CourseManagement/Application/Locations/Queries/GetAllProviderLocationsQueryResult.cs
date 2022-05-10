using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using System.Collections.Generic;

namespace SFA.DAS.Roatp.CourseManagement.Application.Locations.Queries
{
    public class GetAllProviderLocationsQueryResult
    {
        public List<ProviderLocationModel> ProviderLocations { get; set; }
    }
}
