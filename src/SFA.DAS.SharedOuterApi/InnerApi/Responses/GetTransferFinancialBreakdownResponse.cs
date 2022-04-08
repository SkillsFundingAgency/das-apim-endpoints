using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetTransferFinancialBreakdownResponse
    {
        public long Commitments { get; set; }
        public long ApprovedPledgeApplications { get; set; }
        public long AcceptedPledgeApplications { get; set; }
        public long PledgeOriginatedCommitments { get; set; }
        public long TransferConnections { get; set; }
    }
}
