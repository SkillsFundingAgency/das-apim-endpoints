using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetMoreDetails
{
    public class GetMoreDetailsQuery : IRequest<GetMoreDetailsQueryResult>
    {
        public int OpportunityId { get; set; }
    }
}
