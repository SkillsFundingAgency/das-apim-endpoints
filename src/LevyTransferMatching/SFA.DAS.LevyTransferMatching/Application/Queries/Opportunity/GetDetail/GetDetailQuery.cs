using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetDetail
{
    public class GetDetailQuery : IRequest<GetDetailQueryResult>
    {
        public int OpportunityId { get; set; }
    }
}
