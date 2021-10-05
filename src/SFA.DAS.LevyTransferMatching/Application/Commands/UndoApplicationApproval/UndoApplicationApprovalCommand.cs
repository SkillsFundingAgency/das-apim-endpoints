using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge
{
    public class UndoApplicationApprovalCommand : IRequest<UndoApplicationApprovalResult>
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
        public int Amount { get; set; }
    }
}
