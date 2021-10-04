using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.AcceptFunding
{
    public class AcceptFundingCommandHandler : IRequestHandler<AcceptFundingCommand, bool>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<AcceptFundingCommandHandler> _logger;

        public AcceptFundingCommandHandler(ILogger<AcceptFundingCommandHandler> logger, ILevyTransferMatchingService levyTransferMatchingService)
        {
            _logger = logger;
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<bool> Handle(AcceptFundingCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Accepting funding for Application {request.ApplicationId}.");

            var apiRequestData = new AcceptFundingRequestData
            {
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName,
                AccountId = request.AccountId,
                ApplicationId = request.ApplicationId
            };

            var apiRequest = new AcceptFundingRequest(request.ApplicationId, request.AccountId, apiRequestData);

            var result = await _levyTransferMatchingService.AcceptFunding(apiRequest, cancellationToken);

            return result.StatusCode == HttpStatusCode.NoContent;
        }
    }
}
