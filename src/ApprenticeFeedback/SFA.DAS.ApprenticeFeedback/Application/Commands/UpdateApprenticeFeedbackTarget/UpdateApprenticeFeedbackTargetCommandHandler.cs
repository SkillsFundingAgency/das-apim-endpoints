using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.ApprenticeFeedback.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.UpdateApprenticeFeedbackTarget
{
    public class UpdateApprenticeFeedbackTargetCommandHandler : IRequestHandler<UpdateApprenticeFeedbackTargetCommand, UpdateApprenticeFeedbackTargetResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IApprenticeshipDetailsService _apprenticeshipDetailsService;
        private readonly ILogger<UpdateApprenticeFeedbackTargetCommandHandler> _logger;

        public UpdateApprenticeFeedbackTargetCommandHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IApprenticeshipDetailsService apprenticeshipDetailsService,
            ILogger<UpdateApprenticeFeedbackTargetCommandHandler> logger)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _apprenticeshipDetailsService = apprenticeshipDetailsService;
            _logger = logger;
        }

        public async Task<UpdateApprenticeFeedbackTargetResponse> Handle(UpdateApprenticeFeedbackTargetCommand command, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Begin GetApprenticeshipTrainingProviderQueryHandler for ApprenticeId: {command.ApprenticeId}");

            var apprenticeFeedbackTargets = await _apprenticeFeedbackApiClient.
                GetAll<ApprenticeFeedbackTarget>(new GetAllApprenticeFeedbackTargetsRequest { ApprenticeId = command.ApprenticeId });

            if (apprenticeFeedbackTargets == null || apprenticeFeedbackTargets.Any() == false)
            {
                var responseMessage = $"No ApprenticeFeedbackTargets found for ApprenticeId: {command.ApprenticeId}";
                _logger.LogWarning(responseMessage);

                // No feedback targets for the signed in apprentice id
                // QF-349 ticket - https://skillsfundingagency.atlassian.net/browse/QF-349
                // Raised to potentially query apprentice commitments / account api 
                // For now we should return an error to allow the UX to show a relevant page.

                return new UpdateApprenticeFeedbackTargetResponse { };
            }

            var updateApprenticeFeedbackTargetRequests = new List<UpdateApprenticeFeedbackTargetRequest>();
            
            _logger.LogDebug($"Processing feedback targets for: {command.ApprenticeId}");
            foreach (var feedbackTarget in apprenticeFeedbackTargets)
            {
                var apprenticeshipDetails = await _apprenticeshipDetailsService.Get(feedbackTarget.ApprenticeId, feedbackTarget.ApprenticeshipId);

                if (apprenticeshipDetails.LearnerData == null && apprenticeshipDetails.MyApprenticeshipData == null)
                {
                    return new UpdateApprenticeFeedbackTargetResponse();
                }

                updateApprenticeFeedbackTargetRequests.Add(new UpdateApprenticeFeedbackTargetRequest(
                    new UpdateApprenticeFeedbackTargetRequestData
                    {
                        ApprenticeFeedbackTargetId = feedbackTarget.Id,
                        Learner = apprenticeshipDetails.LearnerData,
                        MyApprenticeship = apprenticeshipDetails.MyApprenticeshipData
                    }));
            }

            _logger.LogDebug($"Processing aggregated learners to update the feedback targets for ApprenticeId: {command.ApprenticeId}");
            foreach (var updateRequest in updateApprenticeFeedbackTargetRequests)
            {
                _logger.LogDebug($"Updating Feedback Target with Id: {updateRequest.Data.ApprenticeFeedbackTargetId} for ApprenticeId: {command.ApprenticeId}");
                var response = await _apprenticeFeedbackApiClient.PostWithResponseCode<UpdateApprenticeFeedbackTargetRequestData, ApprenticeFeedbackTarget>(updateRequest);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"Error Updating the apprentice feedback target with Id: {updateRequest.Data.ApprenticeFeedbackTargetId} for apprenticeId: {command.ApprenticeId}, Content: {response.ErrorContent}");
                }
            }

            return new UpdateApprenticeFeedbackTargetResponse();
        }
    }
}
