using MediatR;

namespace SFA.DAS.Approvals.Application.SelectDirectTransferConnection.Queries
{
    public class GetSelectDirectTransferConnectionQuery : IRequest<GetSelectDirectTransferConnectionQueryResult>
    {
        public long AccountId { get; set; }
    }
}