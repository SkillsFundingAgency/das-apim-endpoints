using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourseLocation
{
    public class GetProviderCourseLocationResult
    {
        public List<ProviderCourseLocationModel> ProviderCourseLocations { get; set; } = new List<ProviderCourseLocationModel>();
    }
}
