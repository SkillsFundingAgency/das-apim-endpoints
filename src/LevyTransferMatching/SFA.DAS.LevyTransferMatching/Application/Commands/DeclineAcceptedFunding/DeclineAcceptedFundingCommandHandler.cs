using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.DeclineAcceptedFunding;

public class DeclineAcceptedFundingCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<DeclineAcceptedFundingCommandHandler> logger) : IRequestHandler<DeclineAcceptedFundingCommand, Unit>
{
    public async Task<Unit> Handle(DeclineAcceptedFundingCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Declining Accepted Application {id}", request.ApplicationId);

        var apiRequestData = new DeclineAcceptedFundingRequestData
        {
            ApplicationId = request.ApplicationId,
            UserId = string.Empty,
            UserDisplayName = string.Empty
        };

        var declineRequest = new DeclineAcceptedFundingRequest(request.ApplicationId, apiRequestData);

        await levyTransferMatchingService.DeclineAcceptedFunding(declineRequest);

        return Unit.Value;
    }
}