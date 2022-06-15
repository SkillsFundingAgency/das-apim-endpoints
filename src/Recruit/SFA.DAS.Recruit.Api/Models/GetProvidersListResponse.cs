using System.Collections.Generic;

namespace SFA.DAS.Recruit.Api.Models
{
    public class GetProvidersListResponse
    {
        public IEnumerable<GetProviderResponse> Providers { get; set; }
    }
}