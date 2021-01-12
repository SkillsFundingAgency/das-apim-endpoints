using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetProvidersListResponse
    {
        public IEnumerable<GetProvidersListItem> Providers { get; set; }
    }
}