using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetRoatpProvidersListResponse
    {
        public IEnumerable<GetProvidersListItem> RegisteredProviders { get; set; }
    }
}