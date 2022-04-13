using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetTransferFinancialBreakdownResponse
    {
        public long AccountId { get; set; }
        public DateTime ProjectionStartDate { get; set; }
        public int NumberOfMonths { get; set; }
        public List<BreakdownDetails> Breakdown { get; set; }
        public class BreakdownDetails
        {
            public int Month { get; set; }
            public int Year { get; set; }
            public long FundsIn { get; set; }
            public FundsDetails FundsOut { get; set; }
        }
        public class FundsDetails
        {
            [JsonProperty("approved-pledge-applications")]
            public long ApprovedPledgeApplications { get; set; }
            [JsonProperty("accepted-pledge-applications")]
            public long AcceptedPledgeApplications { get; set; }
            [JsonProperty("pledge-originated-commitments")]
            public long PledgeOriginatedCommitments { get; set; }            
            public long Commitments { get; set; }            
            [JsonProperty("transfer-connections")]
            public long TransferConnections { get; set; }

        }
    }
}
