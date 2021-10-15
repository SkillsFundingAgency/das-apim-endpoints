using System.Collections.Generic;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.GetPledgesResponse;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.GetPledges
{
    public class GetPledgesQueryResult
    {
        public IEnumerable<Pledge> Pledges { get; set; }
        public int TotalPledges { get; set; }
    }
}