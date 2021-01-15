using System.Collections.Generic;

namespace SFA.DAS.ManageApprenticeships.InnerApi.Responses
{
    public class GetFrameworksListResponse
    {
        public IEnumerable<GetFrameworksListItem> Frameworks { get; set; }
    }
}