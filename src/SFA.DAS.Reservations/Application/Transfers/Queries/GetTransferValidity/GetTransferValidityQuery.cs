using MediatR;

namespace SFA.DAS.Reservations.Application.Transfers.Queries.GetTransferValidity
{
    public class GetTransferValidityQuery : IRequest<GetTransferValidityQueryResult>
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public int? PledgeApplicationId { get; set; }
    }
}
