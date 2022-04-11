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
            public long Commitments { get; set; }
            public long ApprovedPledgeApplications { get; set; }
            public long AcceptedPledgeApplications { get; set; }
            public long PledgeOriginatedCommitments { get; set; }
            public long TransferConnections { get; set; }

        }
    }
}
