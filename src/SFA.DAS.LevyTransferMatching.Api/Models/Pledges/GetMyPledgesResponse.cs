using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class GetMyPledgesResponse
    {
        public IEnumerable<MyPledge> Pledges { get; set; }

        public class MyPledge
        {
            public int Id { get; set; }
            public int Amount { get; set; }
        }
    }
}
