using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Reservations.Queries;

public class GetAccountReservationsStatusQueryHandler : IRequestHandler<GetAccountReservationsStatusQuery, GetAccountReservationsStatusQueryResult>
{
    private readonly IReservationApiClient<ReservationApiConfiguration> _apiClient;

    public GetAccountReservationsStatusQueryHandler(IReservationApiClient<ReservationApiConfiguration> apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetAccountReservationsStatusQueryResult> Handle(GetAccountReservationsStatusQuery request, CancellationToken cancellationToken)
    {
        var statusTask = _apiClient.Get<GetAccountReservationsStatusResponse>(new GetAccountReservationsStatusRequest(request.AccountId, request.TransferSenderId));
        var reservationsTask = _apiClient.GetAll<GetAccountReservationItem>(new GetAccountReservationsRequest(request.AccountId));

        await Task.WhenAll(statusTask, reservationsTask);

        var status = await statusTask;
        var reservations = await reservationsTask;

        return new GetAccountReservationsStatusQueryResult
        {
            CanAutoCreateReservations = status.CanAutoCreateReservations,
            HasReachedReservationsLimit = status.HasReachedReservationsLimit,
            UnallocatedPendingReservations = reservations.Count(x=>!x.IsExpired && x.Status == ReservationStatus.Pending)
        };
    }
}