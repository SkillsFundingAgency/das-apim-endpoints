using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AutoApproveApplication
{
    public class AutoApproveApplicationCommand : IRequest
    {
        public int ApplicationId { get; set; }
        public int PledgeId { get; set; }
    }
}
