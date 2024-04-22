using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.ApprenticeFeedback.Models;
using SFA.DAS.ApprenticeFeedback.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate
{
    public class TriggerFeedbackTargetUpdateCommandHandler : IRequestHandler<TriggerFeedbackTargetUpdateCommand, TriggerFeedbackTargetUpdateResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;
        private readonly IApprenticeshipDetailsService _apprenticeshipDetailsService;
        private readonly ILogger<TriggerFeedbackTargetUpdateCommandHandler> _logger;

        public TriggerFeedbackTargetUpdateCommandHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient,
            IApprenticeshipDetailsService apprenticeshipDetailsService,
            ILogger<TriggerFeedbackTargetUpdateCommandHandler> logger)
        {
            _feedbackApiClient = feedbackApiClient;
            _apprenticeshipDetailsService = apprenticeshipDetailsService;
            _logger = logger;
        }

        public async Task<TriggerFeedbackTargetUpdateResponse> Handle(TriggerFeedbackTargetUpdateCommand command, CancellationToken cancellationToken)
        {
            var apprenticeshipDetails = await _apprenticeshipDetailsService.Get(command.ApprenticeId, command.ApprenticeshipId);
            
            if(apprenticeshipDetails.LearnerData == null && apprenticeshipDetails.MyApprenticeshipData == null)
            {
                _logger.LogInformation($"Unable to retrieve my apprenticeship for ApprenticeId: {command.ApprenticeId} or learner for apprentice commitments Id: {command.ApprenticeshipId}");
                return await DeferUpdateApprenticeFeedbackTarget(command.ApprenticeFeedbackTargetId);
            }

            return await UpdateApprenticeFeedbackTarget(command.ApprenticeFeedbackTargetId, apprenticeshipDetails);
        }

        private async Task<TriggerFeedbackTargetUpdateResponse> DeferUpdateApprenticeFeedbackTarget(Guid apprenticeFeedbackTargetId)
        {
            var request = new UpdateApprenticeFeedbackTargetDeferRequest(
                new UpdateApprenticeFeedbackTargetDeferRequestData
                {
                    ApprenticeFeedbackTargetId = apprenticeFeedbackTargetId,
                });

            var response = await _feedbackApiClient.PostWithResponseCode<UpdateApprenticeFeedbackTargetDeferRequestData, ApprenticeFeedbackTarget>(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = $"Error deferring update to the apprentice feedback target with ApprenticeFeedbackTargetId: {apprenticeFeedbackTargetId}, Content: {response.ErrorContent}";
                _logger.LogError(message);
                return new TriggerFeedbackTargetUpdateResponse()
                {
                    Success = false,
                    Message = message
                };
            }

            return new TriggerFeedbackTargetUpdateResponse()
            {
                Success = true
            };
        }

        private async Task<TriggerFeedbackTargetUpdateResponse> UpdateApprenticeFeedbackTarget(Guid apprenticeFeedbackTargetId, ApprenticeshipDetails apprenticeshipDetails)
        {
            var updateApprenticeFeedbackTargetRequest = new UpdateApprenticeFeedbackTargetRequest(
                new UpdateApprenticeFeedbackTargetRequestData
                {
                    ApprenticeFeedbackTargetId = apprenticeFeedbackTargetId,
                    Learner = apprenticeshipDetails.LearnerData,
                    MyApprenticeship = apprenticeshipDetails.MyApprenticeshipData
                });

            var response = await _feedbackApiClient.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(updateApprenticeFeedbackTargetRequest);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = $"Error updating the apprentice feedback target with ApprenticeFeedbackTargetId: {apprenticeFeedbackTargetId}, Content: {response.ErrorContent}";
                _logger.LogError(message);
                return new TriggerFeedbackTargetUpdateResponse()
                {
                    Success = false,
                    Message = message
                };
            }

            return new TriggerFeedbackTargetUpdateResponse()
            {
                Success = true
            };
        }
    }
}
