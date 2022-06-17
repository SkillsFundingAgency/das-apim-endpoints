using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using System.Collections.Generic;

namespace SFA.DAS.Roatp.CourseManagement.Application.Regions.Queries
{
    public class GetAllRegionsQueryResult
    {
        public List<RegionModel> Regions { get; set; }
    }
}
