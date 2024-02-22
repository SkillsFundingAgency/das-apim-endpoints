using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationApprovalOptions
{
    public class SetApplicationApprovalOptionsCommand : IRequest
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public bool AutomaticApproval { get; set; }
    }
}
