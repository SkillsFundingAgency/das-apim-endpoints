using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectPledgeApplications
{
    public class RejectPledgeApplicationsCommand :IRequest
    {
        public int PledgeId { get; set; }
    }
}
