using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetOpportunityApply
{
    public class GetOpportunityApplyQuery : IRequest<GetOpportunityApplyQueryResult>
    {
        public string UserId { get; set; }
    }
}