using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerFinance;

namespace SFA.DAS.Approvals.Application.SelectDirectTransferConnection.Queries;

public class GetSelectDirectTransferConnectionQueryResult
{
    public bool IsLevyAccount { get; set; }
    public IEnumerable<GetTransferConnectionsResponse.TransferConnection> TransferConnections { get; set; }
}
