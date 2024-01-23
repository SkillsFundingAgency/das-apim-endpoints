using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance
{
    public class GetTransferConnectionsResponse
    {
        public IEnumerable<TransferConnection> TransferConnections { get; set; }
        public class TransferConnection
        {
            public long FundingEmployerAccountId { get; set; }
            public string FundingEmployerHashedAccountId { get; set; }
            public string FundingEmployerPublicHashedAccountId { get; set; }
            public string FundingEmployerAccountName { get; set; }
        }
    }
}