using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication
{
    public class RejectApplicationCommand : IRequest
    {
        public int ApplicationId { get; set; }
        public int PledgeId { get; set; }
    }
}
