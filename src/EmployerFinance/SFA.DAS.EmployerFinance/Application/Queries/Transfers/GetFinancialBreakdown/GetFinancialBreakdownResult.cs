using System;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetFinancialBreakdown
{
    public class GetFinancialBreakdownResult
    {
        public decimal ApprovedPledgeApplications { get; set; }
        public decimal AcceptedPledgeApplications { get; set; }
        public decimal TransferConnections { get; set; }
        public decimal PledgeOriginatedCommitments { get; set; }
        public decimal Commitments { get; set; }
        public decimal AmountPledged { get; set; }
    }
}
