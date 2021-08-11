using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetApply
{
    public class GetApplyQuery : IRequest<GetApplyQueryResult>
    {
        public int OpportunityId { get; set; }
    }
}
