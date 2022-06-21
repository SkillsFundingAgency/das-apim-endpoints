using System.Collections.Generic;

namespace SFA.DAS.ManageApprenticeships.Api.Models
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardResponse> Standards { get; set; }
    }
}