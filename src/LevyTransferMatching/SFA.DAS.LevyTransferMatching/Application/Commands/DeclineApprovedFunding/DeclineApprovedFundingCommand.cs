using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DeclineApprovedFunding;

public class DeclineApprovedFundingCommand : IRequest<Unit>
{
    public int ApplicationId { get; set; }
}
