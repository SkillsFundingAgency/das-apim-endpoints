using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetOpportunity
{
    public class GetOpportunityQuery : IRequest<GetOpportunityResult>
    {
        public int OpportunityId { get; set; }
    }
}