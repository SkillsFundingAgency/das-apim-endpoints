using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CleanupPledgeForNonLevy;

public class CleanupPledgeForNonLevyCommand : IRequest<Unit>
{
    public long AccountId { get; set; }
    public int PledgeId { get; set; }
}
