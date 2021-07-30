using System.Collections.Generic;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.GetPledgesResponse;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesResult : List<Pledge>
    {
        public GetPledgesResult(IEnumerable<Pledge> collection) : base(collection)
        {
        }
    }
}