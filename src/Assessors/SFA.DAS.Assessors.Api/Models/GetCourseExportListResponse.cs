using System.Collections.Generic;

namespace SFA.DAS.Assessors.Api.Models
{
    public class GetCourseExportListResponse
    {
        public IEnumerable<GetStandardDetailsResponse> Courses { get; set; }
    }
}