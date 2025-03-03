using System.Collections.Generic;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance.GetTransferConnectionsResponse;

namespace SFA.DAS.Approvals.Application.SelectDirectTransferConnection.Queries;

public class GetSelectDirectTransferConnectionQueryResult
{
    public bool IsLevyAccount { get; set; }
    public IEnumerable<TransferConnection> TransferConnections { get; set; }
}
