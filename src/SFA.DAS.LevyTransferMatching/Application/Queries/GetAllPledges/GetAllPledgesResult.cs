using SFA.DAS.LevyTransferMatching.Models;
using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetAllPledges
{
    public class GetAllPledgesResult : List<Pledge>
    {
        public GetAllPledgesResult(IEnumerable<Pledge> collection) : base(collection)
        {
        }
    }
}
