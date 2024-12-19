using MediatR;

namespace SFA.DAS.Approvals.Application.SelectFunding.Queries
{
    public class GetSelectFundingOptionsQuery : IRequest<GetSelectFundingOptionsQueryResult>
    {
        public long AccountId { get; set; }
    }
}