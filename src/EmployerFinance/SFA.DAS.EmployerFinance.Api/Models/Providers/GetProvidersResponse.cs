using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.Api.Models
{
    public class GetProvidersResponse 

    {
        public IEnumerable<ProviderResponse> Providers { get; set; }
    }
}