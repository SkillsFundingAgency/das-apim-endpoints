using MediatR;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetIndex
{
    public class GetIndexQuery : IRequest<GetIndexQueryResult>
    {
        public long AccountId { get; set; }
    }
}
