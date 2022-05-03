using MediatR;

namespace SFA.DAS.ManageApprenticeships.Application.Queries.Transfers.GetFinancialBreakdown
{
    public class GetFinancialBreakdownQuery : IRequest<GetFinancialBreakdownResult>
    {
        public long AccountId { get; set; }
    }
}
