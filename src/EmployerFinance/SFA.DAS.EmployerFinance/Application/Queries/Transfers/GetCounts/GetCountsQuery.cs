using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetCounts
{
    public class GetCountsQuery : IRequest<GetCountsQueryResult>
    {
        public long AccountId { get; set; }
    }
}
