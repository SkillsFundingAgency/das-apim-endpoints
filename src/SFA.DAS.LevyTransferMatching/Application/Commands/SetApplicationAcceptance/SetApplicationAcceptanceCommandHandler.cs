using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;

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
            _logger.LogInformation($"");

            if (request.Acceptance != Types.ApplicationAcceptance.Accept)
            {
                throw new NotImplementedException();
            }

            _logger.LogInformation($"Accepting funding for Application {request.ApplicationId}. {request}");

            var apiRequestData = new AcceptFundingRequestData
            {
                UserId = request.UserId,
                UserDisplayName = request.UserDisplayName,
                AccountId = request.AccountId,
                ApplicationId = request.ApplicationId
            };

            var apiRequest = new AcceptFundingRequest(request.ApplicationId, request.AccountId, apiRequestData);

            _logger.LogInformation($"Passing Accept funding request to the LTM service : {apiRequest}");

            var result = await _levyTransferMatchingService.AcceptFunding(apiRequest, cancellationToken);

            _logger.LogInformation($"Response from the LTM Service : {result}");

            return result.StatusCode == HttpStatusCode.NoContent;
        }
    }
}