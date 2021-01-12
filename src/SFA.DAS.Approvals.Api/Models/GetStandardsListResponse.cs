using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardResponse> Standards { get ; set ; }
    }
}