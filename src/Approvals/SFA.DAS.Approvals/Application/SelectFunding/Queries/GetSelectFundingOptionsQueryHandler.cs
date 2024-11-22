using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.SelectFunding.Queries;
public class GetSelectFundingOptionsQueryHandler : IRequestHandler<GetSelectFundingOptionsQuery, GetSelectFundingOptionsQueryResult>
{
    private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;
    private readonly IReservationApiClient<ReservationApiConfiguration> _reservationsApiClient;
    private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

    public GetSelectFundingOptionsQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient,
        IReservationApiClient<ReservationApiConfiguration> reservationsApiClient,
        IAccountsApiClient<AccountsConfiguration> accountsApiClient)
    {
        _financeApiClient = financeApiClient;
        _reservationsApiClient = reservationsApiClient;
        _accountsApiClient = accountsApiClient;
    }

    public async Task<GetSelectFundingOptionsQueryResult> Handle(GetSelectFundingOptionsQuery request, CancellationToken cancellationToken)
    {
        var statusTask = _reservationsApiClient.Get<GetAccountReservationsStatusResponse>(new GetAccountReservationsStatusRequest(request.AccountId, null));
        var connectionsTask = _financeApiClient.Get<GetTransferConnectionsResponse>(new GetTransferConnectionsRequest { AccountId = request.AccountId});
        var accountTask = _accountsApiClient.Get<GetAccountResponse>(new GetAccountRequest(request.AccountId.ToString()));

        await Task.WhenAll(statusTask, connectionsTask, accountTask);

        var status = await statusTask;
        var connections = await connectionsTask;
        var account = await accountTask;

        return new GetSelectFundingOptionsQueryResult
        {
            IsLevyAccount = account.ApprenticeshipEmployerType.Equals("Levy", StringComparison.InvariantCultureIgnoreCase),
            HasDirectTransfersAvailable = connections.TransferConnections?.Any() ?? false,
            HasAdditionalReservationFundsAvailable = !status.HasReachedReservationsLimit,
            HasUnallocatedReservationsAvailable = status.HasPendingReservations
        };
    }
}