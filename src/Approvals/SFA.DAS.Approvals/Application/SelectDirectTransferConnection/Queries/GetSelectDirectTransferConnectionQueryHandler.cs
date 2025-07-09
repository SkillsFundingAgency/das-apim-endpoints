using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.SelectDirectTransferConnection.Queries;
public class GetSelectDirectTransferConnectionQueryHandler(
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient,
    IAccountsApiClient<AccountsConfiguration> accountsApiClient)
    : IRequestHandler<GetSelectDirectTransferConnectionQuery, GetSelectDirectTransferConnectionQueryResult>
{
    public async Task<GetSelectDirectTransferConnectionQueryResult> Handle(GetSelectDirectTransferConnectionQuery request, CancellationToken cancellationToken)
    {
        var accountTask = accountsApiClient.Get<GetAccountResponse>(new GetAccountRequest(request.AccountId.ToString()));
        var connectionsTask = financeApiClient.Get<IEnumerable<GetTransferConnectionsResponse.TransferConnection>>(new GetTransferConnectionsRequest { AccountId = request.AccountId });

        await Task.WhenAll(accountTask, connectionsTask);

        var account = await accountTask;
        var connections = await connectionsTask;

        return new GetSelectDirectTransferConnectionQueryResult
        {
            IsLevyAccount = account.ApprenticeshipEmployerType.Equals("Levy", StringComparison.CurrentCultureIgnoreCase),
            TransferConnections = connections
        };
    }
}