using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Opportunities.GetConfirmation
{
    public class GetConfirmationQuery : IRequest<GetConfirmationQueryResult>
    {
        public int OpportunityId { get; set; }
    }
}
