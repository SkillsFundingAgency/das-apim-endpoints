using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllStandardRegions
{
    public class GetAllStandardRegionsQueryResult
    {
        public List<RegionModel> Regions { get; set; } = new List<RegionModel>();
    }
}
