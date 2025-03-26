using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.Rules.Queries.GetAvailableDates
{
    public class GetAvailableDatesQueryHandler(
        IReservationApiClient<ReservationApiConfiguration> reservationApiClient,
        ILogger<GetAvailableDatesQueryHandler> logger)
        : IRequestHandler<GetAvailableDatesQuery, GetAvailableDatesResult>
    {
        public async Task<GetAvailableDatesResult> Handle(GetAvailableDatesQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting Available Dates from Reservation api");

            var innerApiResponse = await reservationApiClient.Get<GetAvailableDatesResponse>(new GetAvailableDatesRequest(request.AccountLegalEntityId));

            return new GetAvailableDatesResult
            {
                AvailableDates = innerApiResponse.AvailableDates
            };
        }
    }
}
