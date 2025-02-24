﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ExpireAcceptedFunding;

public class ExpireAcceptedFundingCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<ExpireAcceptedFundingCommandHandler> logger) : IRequestHandler<ExpireAcceptedFundingCommand, Unit>
{
    public async Task<Unit> Handle(ExpireAcceptedFundingCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Expiring Approved Application {id}", request.ApplicationId);

        var apiRequestData = new ExpireAcceptedFundingRequestData
        {
            ApplicationId = request.ApplicationId,
            UserId = string.Empty,
            UserDisplayName = string.Empty
        };

        var expireRequest = new ExpireAcceptedFundingRequest(request.ApplicationId, apiRequestData);

        await levyTransferMatchingService.ExpireAcceptedFunding(expireRequest);

        return Unit.Value;
    }
}