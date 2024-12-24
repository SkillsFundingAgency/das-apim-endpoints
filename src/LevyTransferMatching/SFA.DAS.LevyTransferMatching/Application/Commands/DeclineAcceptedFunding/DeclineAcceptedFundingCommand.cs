using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DeclineAcceptedFunding;

public class DeclineAcceptedFundingCommand : IRequest<Unit>
{
    public int ApplicationId { get; set; }
}
