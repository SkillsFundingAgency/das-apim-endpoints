using MediatR;

namespace SFA.DAS.Approvals.Application.LevyTransferMatching.Queries
{
    public class GetPledgeApplicationQuery : IRequest<GetPledgeApplicationResult>
    {
        public int PledgeApplicationId { get; set; }
    }
}