using System;
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
public class GetSelectDirectTransferConnectionQueryHandler : IRequestHandler<GetSelectDirectTransferConnectionQuery, GetSelectDirectTransferConnectionQueryResult>
{
    private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;
    private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

    public GetSelectDirectTransferConnectionQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient, IAccountsApiClient<AccountsConfiguration> accountsApiClient)
    {
        _financeApiClient = financeApiClient;
        _accountsApiClient = accountsApiClient;
    }
    public async Task<GetSelectDirectTransferConnectionQueryResult> Handle(GetSelectDirectTransferConnectionQuery request, CancellationToken cancellationToken)
    {
        var accountTask = _accountsApiClient.Get<GetAccountResponse>(new GetAccountRequest(request.AccountId.ToString()));
        var connectionsTask = _financeApiClient.Get<GetTransferConnectionsResponse>(new GetTransferConnectionsRequest { AccountId = request.AccountId});

        await Task.WhenAll(accountTask, connectionsTask);

        var account = await accountTask;
        var connections = await connectionsTask;

        return new GetSelectDirectTransferConnectionQueryResult
        {
            IsLevyAccount = account.ApprenticeshipEmployerType.Equals("Levy", StringComparison.CurrentCultureIgnoreCase),
            TransferConnections = connections.TransferConnections
        };
    }
}