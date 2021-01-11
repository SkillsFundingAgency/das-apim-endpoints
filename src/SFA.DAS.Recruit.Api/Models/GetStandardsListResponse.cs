using System.Collections.Generic;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardResponse> Standards { get; set; }
    }
}