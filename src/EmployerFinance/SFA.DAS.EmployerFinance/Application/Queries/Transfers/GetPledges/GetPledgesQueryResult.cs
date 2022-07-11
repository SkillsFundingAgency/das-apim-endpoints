using System.Collections.Generic;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.GetPledgesResponse;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetPledges
{
    public class GetPledgesQueryResult
    {
        public IEnumerable<Pledge> Pledges { get; set; }
        public int TotalPledges { get; set; }
    }
}