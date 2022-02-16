using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges
{
    public class GetPledgesQueryResult
    {
        public IEnumerable<Pledge> Pledges { get; set; }

        public class Pledge
        {
            public int Id { get; set; }
            public int Amount { get; set; }
            public int RemainingAmount { get; set; }
            public int ApplicationCount { get; set; }
            public string Status { get; set; }
        }
    }
}
