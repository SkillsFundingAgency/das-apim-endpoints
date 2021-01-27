using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public class GetFrameworksListResponse
    {
        public IEnumerable<GetFrameworksListItem> Frameworks { get; set; }
    }
}