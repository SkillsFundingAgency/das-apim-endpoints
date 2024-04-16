using MediatR;

namespace SFA.DAS.Reservations.Application.Rules.Queries.GetAvailableDates
{
    public class GetAvailableDatesQuery : IRequest<GetAvailableDatesResult>
    {
        public long AccountLegalEntityId { get; set; }
    }
}
