using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses
{
    public class GetProvidersListResponse
    {
        public IEnumerable<GetProvidersListItem> Providers { get; set; }
        
    }
}