using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.TriggerFeedbackTargetUpdate
{
    public class TriggerFeedbackTargetUpdateCommandHandler : IRequestHandler<TriggerFeedbackTargetUpdateCommand, TriggerFeedbackTargetUpdateResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _feedbackApiClient;

        public TriggerFeedbackTargetUpdateCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> feedbackApiClient)
        {
            _feedbackApiClient = feedbackApiClient;
        }

        public async Task<TriggerFeedbackTargetUpdateResponse> Handle(TriggerFeedbackTargetUpdateCommand command, CancellationToken cancellationToken)
        {
            var updateApprenticeFeedbackTargetRequest = new UpdateApprenticeFeedbackTargetRequest(
                new UpdateApprenticeFeedbackTargetRequestData
                {
                    ApprenticeFeedbackTargetId = command.Id,
                    //Learner = (LearnerData)aggregate.Learner,
                }
            );

            var response = await _feedbackApiClient.PostWithResponseCode<ApprenticeFeedbackTarget>(updateApprenticeFeedbackTargetRequest);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
               // _logger.LogError($"Error Updating the apprentice feedback target with Id: {aggregate.ApprenticeFeedbackTargetId} for apprenticeId: {command.ApprenticeId}, Content: {response.ErrorContent}");
            }

            return new TriggerFeedbackTargetUpdateResponse
            {
                // something
            };
        }
    }
}
