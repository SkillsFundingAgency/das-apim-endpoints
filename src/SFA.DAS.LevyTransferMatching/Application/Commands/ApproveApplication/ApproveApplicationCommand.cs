using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication
{
    public class ApproveApplicationCommand : IRequest
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}
