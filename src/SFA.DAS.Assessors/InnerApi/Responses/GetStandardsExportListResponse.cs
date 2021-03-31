using System.Collections.Generic;

namespace SFA.DAS.Assessors.InnerApi.Responses
{
    public class GetStandardsExportListResponse
    {
        public IEnumerable<StandardDetailResponse> Courses { get; set; }
    }
}