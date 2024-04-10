using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectPledgeApplications
{
    public class RejectPledgeApplicationsCommand :IRequest<Unit>
    {
        public int PledgeId { get; set; }
    }
}
