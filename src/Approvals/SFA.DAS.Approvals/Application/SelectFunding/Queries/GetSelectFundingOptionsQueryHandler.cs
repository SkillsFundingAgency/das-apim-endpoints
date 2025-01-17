using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.SelectFunding.Queries;
public class GetSelectFundingOptionsQueryHandler : IRequestHandler<GetSelectFundingOptionsQuery, GetSelectFundingOptionsQueryResult>
{
    private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;
    private readonly IReservationApiClient<ReservationApiConfiguration> _reservationsApiClient;
    private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;
    private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _ltmApiClient;

    public GetSelectFundingOptionsQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient,
        IReservationApiClient<ReservationApiConfiguration> reservationsApiClient,
        IAccountsApiClient<AccountsConfiguration> accountsApiClient,
        ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> ltmApiClient)
    {
        _financeApiClient = financeApiClient;
        _reservationsApiClient = reservationsApiClient;
        _accountsApiClient = accountsApiClient;
        _ltmApiClient = ltmApiClient;
    }

    public async Task<GetSelectFundingOptionsQueryResult> Handle(GetSelectFundingOptionsQuery request, CancellationToken cancellationToken)
    {
        var statusTask = _reservationsApiClient.Get<GetAccountReservationsStatusResponse>(new GetAccountReservationsStatusRequest(request.AccountId, null));
        var connectionsTask = _financeApiClient.Get<IEnumerable<GetTransferConnectionsResponse.TransferConnection>>(new GetTransferConnectionsRequest { AccountId = request.AccountId});
        var accountTask = _accountsApiClient.Get<GetAccountResponse>(new GetAccountRequest(request.AccountId.ToString()));
        var ltmTask = _ltmApiClient.Get<GetApplicationsResponse>(new GetAcceptedEmployerAccountPledgeApplicationsRequest(request.AccountId));

        await Task.WhenAll(statusTask, connectionsTask, accountTask, ltmTask);

        var status = await statusTask;
        var connections = await connectionsTask;
        var account = await accountTask;
        var ltm = await ltmTask;

        return new GetSelectFundingOptionsQueryResult
        {
            IsLevyAccount = account.ApprenticeshipEmployerType?.Equals("Levy", StringComparison.InvariantCultureIgnoreCase) ?? false,
            HasDirectTransfersAvailable = connections?.Any() ?? false,
            HasLtmTransfersAvailable = ltm.Applications?.Any() ?? false,
            HasAdditionalReservationFundsAvailable = !status.HasReachedReservationsLimit,
            HasUnallocatedReservationsAvailable = status.HasPendingReservations
        };
    }
}