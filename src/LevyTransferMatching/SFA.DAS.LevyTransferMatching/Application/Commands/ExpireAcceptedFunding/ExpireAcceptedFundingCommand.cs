using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ExpireAcceptedFunding;

public class ExpireAcceptedFundingCommand : IRequest<Unit>
{
    public int ApplicationId { get; set; }
}
