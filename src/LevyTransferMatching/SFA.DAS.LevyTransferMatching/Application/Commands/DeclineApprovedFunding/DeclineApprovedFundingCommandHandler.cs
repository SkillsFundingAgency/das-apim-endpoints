using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DeclineApprovedFunding;

public class DeclineApprovedFundingCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<DeclineApprovedFundingCommandHandler> logger) : IRequestHandler<DeclineApprovedFundingCommand, Unit>
{
    public async Task<Unit> Handle(DeclineApprovedFundingCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Declining Accepted Application {id}", request.ApplicationId);

        var apiRequestData = new DeclineApprovedFundingRequestData
        {
            ApplicationId = request.ApplicationId,
            UserId = string.Empty,
            UserDisplayName = string.Empty
        };

        var declineRequest = new DeclineApprovedFundingRequest(request.ApplicationId, apiRequestData);

        await levyTransferMatchingService.DeclineAcceptedFunding(declineRequest);

        return Unit.Value;
    }
}