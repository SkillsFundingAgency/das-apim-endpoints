using MediatR;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetReservations
{
    public class GetReservationsQuery : IRequest<GetReservationsQueryResult>
    {
        public string AccountId { get; set; }
    }
}