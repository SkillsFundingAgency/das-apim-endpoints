using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.UpdateApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.ApprenticeFeedback.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.PatchApprenticeFeedbackTarget
{
    public class PatchApprenticeFeedbackTargetCommandHandler : IRequestHandler<PatchApprenticeFeedbackTargetCommand, PatchApprenticeFeedbackTargetResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly ILogger<PatchApprenticeFeedbackTargetCommandHandler> _logger;

        public PatchApprenticeFeedbackTargetCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient, ILogger<PatchApprenticeFeedbackTargetCommandHandler> logger)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _logger = logger;
        }

        public async Task<PatchApprenticeFeedbackTargetResponse> Handle(PatchApprenticeFeedbackTargetCommand command, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Begin PatchApprenticeFeedbackTargetCommandHandler for ApprenticeFeedbackTargetId: {command.ApprenticeFeedbackTargetId}");
            var patchRequest = new UpdateApprenticeFeedbackTargetStatusRequest(new UpdateApprenticeFeedbackTargetStatusRequestData()
            {
                ApprenticeFeedbackTargetId = command.ApprenticeFeedbackTargetId,
                Status = command.Status,
                FeedbackEligibilityStatus = command.FeedbackEligibilityStatus
            });
            var response = await _apprenticeFeedbackApiClient.PatchWithResponseCode(patchRequest);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError($"Unable to delete feedback transactions belonging to ApprenticeTargetId {command.ApprenticeFeedbackTargetId} which should now be complete");
            }

            _logger.LogDebug($"Completed PatchApprenticeFeedbackTargetCommandHandler for ApprenticeFeedbackTargetId: {command.ApprenticeFeedbackTargetId}");
            return new PatchApprenticeFeedbackTargetResponse();
        }
    }
}
