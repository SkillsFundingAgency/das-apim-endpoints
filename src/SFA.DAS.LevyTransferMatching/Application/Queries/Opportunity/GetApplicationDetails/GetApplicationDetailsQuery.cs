using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetApplicationDetails
{
    public class GetApplicationDetailsQuery : IRequest<GetApplicationDetailsQueryResult>
    {
        public int OpportunityId { get; set; }
        public string StandardId { get; set; }
    }
}
