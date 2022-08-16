using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetAccountProjectionSummary
{
    public class GetAccountProjectionSummaryQuery : IRequest<GetAccountProjectionSummaryQueryResult>
    {
        public long AccountId { get; set; }
    }
}
