using MediatR;

namespace SFA.DAS.Approvals.Application.Reservations.Queries
{
    public class GetAccountReservationsStatusQuery : IRequest<GetAccountReservationsStatusQueryResult>
    {
        public long AccountId { get; set; }
        public long? TransferSenderId { get; set; }
    }
}