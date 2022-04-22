using System;

namespace SFA.DAS.ManageApprenticeships.Api.Models.Transfers
{
    public class GetFinancialBreakdownResponse
    {
        public decimal Commitments { get; set; }
        public decimal ApprovedPledgeApplications { get; set; }
        public decimal AcceptedPledgeApplications { get; set; }
        public decimal PledgeOriginatedCommitments { get; set; }
        public decimal TransferConnections { get; set; }
        public decimal FundsIn { get; set; }
        public DateTime ProjectionStartDate { get; set; }
        public int NumberOfMonths { get; set; } 
        public decimal AmountPledged { get; set; }
    }

}
