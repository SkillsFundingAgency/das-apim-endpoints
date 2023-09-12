using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetReservations
{
    public class GetReservationsQuery : IRequest<GetReservationsQueryResult>
    {
        public long AccountId { get; set; }
    }
}