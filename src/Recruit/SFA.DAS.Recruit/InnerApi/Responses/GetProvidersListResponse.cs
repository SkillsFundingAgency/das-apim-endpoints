using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public class GetProvidersListResponse
    {
        public IEnumerable<GetProvidersListItem> RegisteredProviders { get; set; }
        
    }
}