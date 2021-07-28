using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetMyPledges
{
    public class GetMyPledgesQueryResult
    {
        public IEnumerable<MyPledge> Pledges { get; set; }

        public class MyPledge
        {
            public int Id { get; set; }
            public int Amount { get; set; }
            public int RemainingAmount { get; set; }
            public int ApplicationCount { get; set; }
        }
    }
}
