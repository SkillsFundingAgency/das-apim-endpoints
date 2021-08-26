using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DebitPledge
{
    public class DebitPledgeCommand : IRequest
    {
        public int PledgeId { get; set; }
        public int Amount { get; set; }
    }
}
