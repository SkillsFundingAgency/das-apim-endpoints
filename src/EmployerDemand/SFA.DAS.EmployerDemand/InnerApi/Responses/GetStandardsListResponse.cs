using System.Collections.Generic;

namespace SFA.DAS.EmployerDemand.InnerApi.Responses
{
    public class GetStandardsListResponse
    {
        public IReadOnlyList<GetStandardsListItem> Standards { get; set; }
    }
}