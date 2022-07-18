using System.Collections.Generic;

namespace SFA.DAS.ManageApprenticeships.Api.Models
{
    public class GetProvidersListResponse
    {
        public IEnumerable<GetProviderResponse> Providers { get; set; }
    }
}