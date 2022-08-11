using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses
{
    public class GetFrameworksListResponse
    {
        public IEnumerable<GetFrameworksListItem> Frameworks { get; set; }
    }
}