using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetEpaosListResponse
    {
        public IEnumerable<GetEpaosListItem> Epaos { get; set; }
    }
}