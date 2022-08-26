using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllStandardRegions
{
    public class GetAllStandardRegionsQueryResult
    {
        public List<StandardRegionModel> Regions { get; set; } = new List<StandardRegionModel>();
    }
}
