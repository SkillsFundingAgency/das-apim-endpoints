using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetFrameworksListResponse
    {
        public IEnumerable<GetFrameworksListItem> Frameworks { get; set; }
    }
}