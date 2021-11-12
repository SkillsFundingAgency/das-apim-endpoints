using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationAcceptance
{
    public class SetApplicationAcceptanceCommandHandler : IRequestHandler<SetApplicationAcceptanceCommand, bool>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<SetApplicationAcceptanceCommandHandler> _logger;

        public SetApplicationAcceptanceCommandHandler(ILogger<SetApplicationAcceptanceCommandHandler> logger, ILevyTransferMatchingService levyTransferMatchingService)
        {
            _logger = logger;
            _levyTransferMatchingService = levyTransferMatchingService;
        }

        public async Task<bool> Handle(SetApplicationAcceptanceCommand request, CancellationToken cancellationToken)
        {
            HttpStatusCode httpStatusCode;

            if (request.Acceptance == Types.ApplicationAcceptance.Accept)
                httpStatusCode = await AcceptFunding(request, cancellationToken);
            else
                httpStatusCode = await DeclineFunding(request);

            return httpStatusCode == HttpStatusCode.NoContent;
        }

        private async Task<HttpStatusCode> AcceptFunding(SetApplicationAcceptanceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Accepting funding for Application {request.ApplicationId}. {request}");

            var apiRequestData = new AcceptFundingRequestData
            {
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName,
                AccountId = request.AccountId,
                ApplicationId = request.ApplicationId
            };

            var apiRequest = new AcceptFundingRequest(request.ApplicationId, request.AccountId, apiRequestData);

            var result = await _levyTransferMatchingService.AcceptFunding(apiRequest, cancellationToken);

            return result.StatusCode;
        }

        private async Task<HttpStatusCode> DeclineFunding(SetApplicationAcceptanceCommand request)
        {
            _logger.LogInformation($"Declining funding for Application {request.ApplicationId}. {request}");

            var apiRequestData = new DeclineFundingRequestData
            {
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName,
                AccountId = request.AccountId,
                ApplicationId = request.ApplicationId
            };

            var apiRequest = new DeclineFundingRequest(request.ApplicationId, request.AccountId, apiRequestData);

            var result = await _levyTransferMatchingService.DeclineFunding(apiRequest);

            return result.StatusCode;
        }
    }
}