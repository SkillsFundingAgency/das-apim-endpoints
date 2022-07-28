using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetTransferFinancialBreakdownResponse
    {
        public long AccountId { get; set; }
        public DateTime ProjectionStartDate { get; set; }        
        public decimal AmountPledged { get; set; }
        public List<BreakdownDetails> Breakdown { get; set; }
        public class BreakdownDetails
        {
            public int Month { get; set; }
            public int Year { get; set; }            
            public FundsDetails FundsOut { get; set; }
        }
        public class FundsDetails
        {
            [JsonPropertyName("approved-pledge-applications")]
            public decimal ApprovedPledgeApplications { get; set; }
            [JsonPropertyName("accepted-pledge-applications")]
            public decimal AcceptedPledgeApplications { get; set; }
            [JsonPropertyName("pledge-originated-commitments")]
            public decimal PledgeOriginatedCommitments { get; set; }            
            public decimal Commitments { get; set; }            
            [JsonPropertyName("transfer-connections")]
            public decimal TransferConnections { get; set; }

        }
    }
}
