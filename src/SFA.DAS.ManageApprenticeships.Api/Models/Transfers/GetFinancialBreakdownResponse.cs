using System;

namespace SFA.DAS.ManageApprenticeships.Api.Models.Transfers
{
    public class GetFinancialBreakdownResponse
    {
        public long Commitments { get; set; }
        public long ApprovedPledgeApplications { get; set; }
        public long AcceptedPledgeApplications { get; set; }
        public long PledgeOriginatedCommitments { get; set; }
        public long TransferConnections { get; set; }
        public long FundsIn { get; set; }
        public DateTime ProjectionStartDate { get; set; }
    }

}
