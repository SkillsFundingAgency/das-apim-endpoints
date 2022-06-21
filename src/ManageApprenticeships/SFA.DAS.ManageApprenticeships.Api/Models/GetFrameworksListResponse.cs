using System.Collections.Generic;

namespace SFA.DAS.ManageApprenticeships.Api.Models
{
    public class GetFrameworksListResponse
    {
        public IEnumerable<GetFrameworkResponse> Frameworks { get; set; }
    }
}