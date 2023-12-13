using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.ApprenticeFeedback.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
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
                return new TriggerFeedbackTargetUpdateResponse()
                {
                    Success = false,
                    Message = $"Unable to retrieve my apprenticeship data Id: {command.ApprenticeId} or learner for apprentice commitments Id: {command.ApprenticeshipId}"
                };
            }
            
            var updateApprenticeFeedbackTargetRequest = new UpdateApprenticeFeedbackTargetRequest(
                new UpdateApprenticeFeedbackTargetRequestData
                {
                    ApprenticeFeedbackTargetId = command.ApprenticeFeedbackTargetId,
                    Learner = apprenticeshipDetails.LearnerData,
                    MyApprenticeship = apprenticeshipDetails.MyApprenticeshipData
                });

            var updateApprenticeFeedbackTargetResponse = await _feedbackApiClient.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(updateApprenticeFeedbackTargetRequest);
            if (updateApprenticeFeedbackTargetResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError($"Error updating the apprentice feedback target with Id: {command.ApprenticeFeedbackTargetId}, Content: {updateApprenticeFeedbackTargetResponse.ErrorContent}");
                return new TriggerFeedbackTargetUpdateResponse()
                {
                    Success = false,
                    Message = updateApprenticeFeedbackTargetResponse.ErrorContent
                };
            }

            return new TriggerFeedbackTargetUpdateResponse()
            {
                Success = true
            };
        }
    }
}
