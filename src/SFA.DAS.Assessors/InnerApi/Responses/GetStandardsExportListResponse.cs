using System.Collections.Generic;

namespace SFA.DAS.Assessors.InnerApi.Responses
{
    public class GetStandardsExportListResponse
    {
        public IEnumerable<StandardDetailResponse> Standards { get; set; }
    }
}