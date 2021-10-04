using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetSector
{
    public class GetSectorQuery : IRequest<GetSectorQueryResult>
    {
        public int OpportunityId { get; set; }
    }
}