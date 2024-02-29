using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApplicationWithdrawnAfterAcceptance
{
    public class ApplicationWithdrawnAfterAcceptanceCommand : IRequest<Unit>
    {
        public int ApplicationId { get; set; }
        public int PledgeId { get; set; }
        public int Amount { get; set; }
    }
}
