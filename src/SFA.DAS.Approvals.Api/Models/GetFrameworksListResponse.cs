using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetFrameworksListResponse
    {
        public IEnumerable<GetFrameworkResponse> Frameworks { get; set; }
    }
}