using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IEnumerable<GetStandardsListItem> Standards { get; set; }
    }
}