using SFA.DAS.LevyTransferMatching.Models;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetPledges
{
    public class GetPledgesResult : List<Pledge>
    {
        public GetPledgesResult(IEnumerable<Pledge> collection) : base(collection)
        {
        }
    }
}