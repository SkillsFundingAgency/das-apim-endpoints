using MediatR;
using SFA.DAS.LevyTransferMatching.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApplicationCreatedForImmediateAutoApproval
{
    public class ApplicationCreatedForImmediateAutoApprovalCommandHandler : IRequestHandler<ApplicationCreatedForImmediateAutoApprovalCommand, Unit>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ILogger<ApplicationCreatedForImmediateAutoApprovalCommandHandler> _logger;

        public ApplicationCreatedForImmediateAutoApprovalCommandHandler(ILevyTransferMatchingService levyTransferMatchingService, ILogger<ApplicationCreatedForImmediateAutoApprovalCommandHandler> logger)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _logger = logger;
        }

        public async Task<Unit> Handle(ApplicationCreatedForImmediateAutoApprovalCommand request, CancellationToken cancellationToken)
        {
            var getApplicationResponse = await _levyTransferMatchingService.GetApplication(new GetApplicationRequest(request.PledgeId, request.ApplicationId));
          
            if (getApplicationResponse.Amount <= getApplicationResponse.PledgeRemainingAmount
                && getApplicationResponse.AutomaticApprovalOption == AutomaticApprovalOption.ImmediateAutoApproval
                && getApplicationResponse.MatchPercentage == 100)
            {
                _logger.LogInformation($"Immediately automatically approving application {request.ApplicationId} for Pledge {request.PledgeId}");

                var apiRequestData = new ApproveApplicationRequestData
                {
                    UserId = string.Empty,
                    UserDisplayName = string.Empty,
                    AutomaticApproval = true
                };

                var apiRequest = new ApproveApplicationRequest(request.PledgeId, request.ApplicationId, apiRequestData);

                await _levyTransferMatchingService.ApproveApplication(apiRequest);
            }

            return Unit.Value;
        }
    }
}
