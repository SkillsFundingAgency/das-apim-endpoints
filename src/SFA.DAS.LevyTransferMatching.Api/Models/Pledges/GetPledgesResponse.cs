using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetPledgesResponse
    {
        public IEnumerable<Pledge> Pledges { get; set; }

        public class Pledge
        {
            public int Id { get; set; }
            public int Amount { get; set; }
            public int RemainingAmount { get; set; }
            public int ApplicationCount { get; set; }
        }
    }
}
