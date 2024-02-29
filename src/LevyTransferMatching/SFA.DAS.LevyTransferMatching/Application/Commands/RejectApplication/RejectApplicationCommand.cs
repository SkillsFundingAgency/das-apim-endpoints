using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication
{
    public class RejectApplicationCommand : IRequest<Unit>
    {
        public int ApplicationId { get; set; }
        public int PledgeId { get; set; }
    }
}
