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

    public GetSelectFundingOptionsQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient, IReservationApiClient<ReservationApiConfiguration> reservationsApiClient)
    {
        _financeApiClient = financeApiClient;
        _reservationsApiClient = reservationsApiClient;
    }
    public async Task<GetSelectFundingOptionsQueryResult> Handle(GetSelectFundingOptionsQuery request, CancellationToken cancellationToken)
    {
        var statusTask = _reservationsApiClient.Get<GetAccountReservationsStatusResponse>(new GetAccountReservationsStatusRequest(request.AccountId, null));
        var connectionsTask = _financeApiClient.Get<GetTransferConnectionsResponse>(new GetTransferConnectionsRequest { AccountId = request.AccountId});

        await Task.WhenAll(statusTask, connectionsTask);

        var status = await statusTask;
        var connections = await connectionsTask;

        return new GetSelectFundingOptionsQueryResult
        {
            IsLevyAccount = status.CanAutoCreateReservations,
            HasDirectTransfersAvailable = connections.TransferConnections?.Any() ?? false,
            HasAdditionalReservationFundsAvailable = !status.HasReachedReservationsLimit,
            HasUnallocatedReservationsAvailable = status.HasPendingReservations
        };
    }
}