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
        public decimal AmountPledged { get; set; }
        public List<BreakdownDetails> Breakdown { get; set; }
        public class BreakdownDetails
        {
            public int Month { get; set; }
            public int Year { get; set; }
            public decimal FundsIn { get; set; }
            public FundsDetails FundsOut { get; set; }
        }
        public class FundsDetails
        {
            [JsonProperty("approved-pledge-applications")]
            public decimal ApprovedPledgeApplications { get; set; }
            [JsonProperty("accepted-pledge-applications")]
            public decimal AcceptedPledgeApplications { get; set; }
            [JsonProperty("pledge-originated-commitments")]
            public decimal PledgeOriginatedCommitments { get; set; }            
            public decimal Commitments { get; set; }            
            [JsonProperty("transfer-connections")]
            public decimal TransferConnections { get; set; }

        }
    }
}
