using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models
{
    public class GetProvidersListResponse
    {
        public IEnumerable<GetProvidersResponse> Providers { get; set; }
    }
}