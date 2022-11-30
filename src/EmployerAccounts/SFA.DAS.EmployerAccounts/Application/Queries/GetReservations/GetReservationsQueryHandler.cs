using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetReservations
{
    public class GetReservationsQueryHandler : IRequestHandler<GetReservationsQuery, GetReservationsQueryResult>
    {
        private readonly IReservationApiClient<ReservationApiConfiguration> _reservationApiClient;

        public GetReservationsQueryHandler(IReservationApiClient<ReservationApiConfiguration> reservationApiClient)
        {
            _reservationApiClient = reservationApiClient;
        }

        public async Task<GetReservationsQueryResult> Handle(GetReservationsQuery request,
            CancellationToken cancellationToken)
        {
            var response =
                await _reservationApiClient.GetAll<GetReservationsResponseListItem>(
                    new GetReservationsRequest(request.AccountId));

            return new GetReservationsQueryResult
            {
                Reservations = response
            };
        }
    }
}