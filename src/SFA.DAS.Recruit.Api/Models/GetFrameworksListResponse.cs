using System.Collections.Generic;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetFrameworksListResponse
    {
        public IEnumerable<GetFrameworkResponse> Frameworks { get; set; }
    }
}