using MediatR;
using Microsoft.Extensions.Logging;
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

namespace SFA.DAS.ApprenticeFeedback.Application.Commands.UpdateApprenticeFeedbackTarget
{
    public class UpdateApprenticeFeedbackTargetCommandHandler : IRequestHandler<UpdateApprenticeFeedbackTargetCommand, UpdateApprenticeFeedbackTargetResponse>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly ILogger<UpdateApprenticeFeedbackTargetCommandHandler> _logger;

        public UpdateApprenticeFeedbackTargetCommandHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            ILogger<UpdateApprenticeFeedbackTargetCommandHandler> logger)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _assessorsApiClient = assessorsApiClient;
            _logger = logger;
        }

        public async Task<UpdateApprenticeFeedbackTargetResponse> Handle(UpdateApprenticeFeedbackTargetCommand command, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Begin GetApprenticeshipTrainingProviderQueryHandler for ApprenticeId: {command.ApprenticeId}");

            // 1. Get all apprentice feedback targets for the given signed in apprentice.
            var apprenticeFeedbackTargets = await _apprenticeFeedbackApiClient.
                GetAll<ApprenticeFeedbackTarget>(new GetAllApprenticeFeedbackTargetsRequest { ApprenticeId = command.ApprenticeId });


            // 1.a If none, we do nothing, but potential for in future to make it smarter.
            if (apprenticeFeedbackTargets == null || apprenticeFeedbackTargets.Any() == false)
            {
                var responseMessage = $"No ApprenticeFeedbackTargets found for ApprenticeId: { command.ApprenticeId}";
                _logger.LogWarning(responseMessage);

                // No feedback targets for the signed in apprentice id
                // QF-349 ticket - https://skillsfundingagency.atlassian.net/browse/QF-349
                // Raised to potentially query apprentice commitments / account api 
                // For now we should return an error to allow the UX to show a relevant page.

                return new UpdateApprenticeFeedbackTargetResponse
                {
                    Success = false,
                    Message = responseMessage
                };
            }

            // 2. Setup Learner aggregate object holder to contain information from external systems to supply to Inner Api.
            var learnerAggregate = new List<ApprenticeLearnerAggregate>();

            // 3. Get corresponding Learner Record for apprentice feedback target record and update aggregate if found.
            _logger.LogDebug($"Processing feedback targets for: {command.ApprenticeId}");
            foreach (var feedbackTarget in apprenticeFeedbackTargets)
            {
                _logger.LogDebug($"Retrieving learner record with apprentice commitments Id: {feedbackTarget.ApprenticeshipId}");
                var learnerResponse = await _assessorsApiClient.GetWithResponseCode<GetApprenticeLearnerResponse>(new GetApprenticeLearnerRequest(feedbackTarget.ApprenticeshipId));

                if (learnerResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"Error Retrieving learner record with apprentice commitments Id: {feedbackTarget.ApprenticeshipId}, Content: {learnerResponse.ErrorContent}");
                    continue;
                }

                learnerAggregate.Add(new ApprenticeLearnerAggregate
                {
                    ApprenticeFeedbackTargetId = feedbackTarget.Id,
                    Learner = learnerResponse.Body
                });
            }

            // 4. Send Update Call to Inner Api with latest information to process and update the feedback target.
            _logger.LogDebug($"Processing aggregated learners to update the feedback targets for ApprenticeId: {command.ApprenticeId}");
            foreach (var aggregate in learnerAggregate)
            {
                _logger.LogDebug($"Updating Feedback Target with Id: {aggregate.ApprenticeFeedbackTargetId} for ApprenticeId: {command.ApprenticeId}");
                var updateApprenticeFeedbackTargetRequest = new UpdateApprenticeFeedbackTargetRequest(
                    new UpdateApprenticeFeedbackTargetRequestData
                    {
                        ApprenticeFeedbackTargetId = aggregate.ApprenticeFeedbackTargetId,
                        // Hard Coded until we know what we're doing with this, hopefully it can be deleted
                        ActiveApprenticeshipsCount = 99,
                        Learner = (LearnerData)aggregate.Learner,
                    });

                var response = await _apprenticeFeedbackApiClient.PostWithResponseCode<ApprenticeFeedbackTarget>(updateApprenticeFeedbackTargetRequest);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"Error Updating the apprentice feedback target with Id: {aggregate.ApprenticeFeedbackTargetId} for apprenticeId: {command.ApprenticeId}, Content: {response.ErrorContent}");
                }
            }

            return new UpdateApprenticeFeedbackTargetResponse
            {
                Success = true
            };
        }
    }
}
