using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate
{
    public class TriggerFeedbackTargetUpdateCommandHandler : IRequestHandler<TriggerFeedbackTargetUpdateCommand, TriggerFeedbackTargetUpdateResponse>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;
        private readonly ILogger<TriggerFeedbackTargetUpdateCommandHandler> _logger;

        public TriggerFeedbackTargetUpdateCommandHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient
            , IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient
            , ILogger<TriggerFeedbackTargetUpdateCommandHandler> logger
            )
        {
            _feedbackApiClient = feedbackApiClient;
            _assessorsApiClient = assessorsApiClient;
            _logger = logger;
        }

        public async Task<TriggerFeedbackTargetUpdateResponse> Handle(TriggerFeedbackTargetUpdateCommand command, CancellationToken cancellationToken)
        {
            // 1. Attempt to get learner info
            var learnerResponse = await _assessorsApiClient.GetWithResponseCode<GetApprenticeLearnerResponse>(new GetApprenticeLearnerRequest(command.ApprenticeshipId));
            if (learnerResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var errorMsg = $"Error retrieving learner record with apprentice commitments Id: {command.ApprenticeshipId}";
                if(!string.IsNullOrWhiteSpace(learnerResponse.ErrorContent))
                {
                    errorMsg += $", Content: {learnerResponse.ErrorContent}"; 
                }
                _logger.LogError(errorMsg);
                return new TriggerFeedbackTargetUpdateResponse()
                {
                    Success = false,
                    Message = string.IsNullOrWhiteSpace(learnerResponse.ErrorContent) ? errorMsg : learnerResponse.ErrorContent,
                };
            }

            // 2. Send Update Call to Inner Api with latest information to process and update the feedback target.
            var updateApprenticeFeedbackTargetRequest = new UpdateApprenticeFeedbackTargetRequest(
                new UpdateApprenticeFeedbackTargetRequestData
                {
                    ApprenticeFeedbackTargetId = command.ApprenticeFeedbackTargetId,
                    Learner = learnerResponse.Body,
                });
            var updateApprenticeFeedbackTargetResponse = await _feedbackApiClient.PostWithResponseCode<ApprenticeFeedbackTarget>(updateApprenticeFeedbackTargetRequest);
            if (updateApprenticeFeedbackTargetResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError($"Error updating the apprentice feedback target with Id: {command.ApprenticeFeedbackTargetId}, Content: {updateApprenticeFeedbackTargetResponse.ErrorContent}");
                return new TriggerFeedbackTargetUpdateResponse()
                {
                    Success = false,
                    Message = updateApprenticeFeedbackTargetResponse.ErrorContent,
                };
            }

            return new TriggerFeedbackTargetUpdateResponse()
            {
                Success = true,
            };
        }
    }
}
