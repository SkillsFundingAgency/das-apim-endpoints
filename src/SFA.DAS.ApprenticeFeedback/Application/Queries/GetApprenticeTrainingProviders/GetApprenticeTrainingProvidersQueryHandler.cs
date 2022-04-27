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

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProviders
{
    public class GetApprenticeTrainingProvidersQueryHandler : IRequestHandler<GetApprenticeTrainingProvidersQuery, GetApprenticeTrainingProvidersResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _apprenticeCommitmentsApiClient;
        private readonly ILogger<GetApprenticeTrainingProvidersQueryHandler> _logger;

        public GetApprenticeTrainingProvidersQueryHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient,
            IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> apprenticeCommitmentsApiClient,
            ILogger<GetApprenticeTrainingProvidersQueryHandler> logger)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _assessorsApiClient = assessorsApiClient;
            _apprenticeCommitmentsApiClient = apprenticeCommitmentsApiClient;
            _logger = logger;
        }

        public async Task<GetApprenticeTrainingProvidersResult> Handle(GetApprenticeTrainingProvidersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Begin GetApprenticeshipTrainingProviderQueryHandler for ApprenticeId: {request.ApprenticeId}");
            // 1. Get all apprentice feedback targets for the given signed in apprentice.
            var apprenticeFeedbackTargets = await _apprenticeFeedbackApiClient.
                GetAll<ApprenticeFeedbackTarget>(new GetAllApprenticeFeedbackTargetsRequest { ApprenticeId = request.ApprenticeId });

            // 1.a If none, we do nothing, but potential for in future to make it smarter.
            if (apprenticeFeedbackTargets?.Count() == 0)
            {
                _logger.LogWarning($"No ApprenticeFeedbackTargets found for ApprenticeId: {request.ApprenticeId}");
                // No feedback targets for the signed in apprentice id
                // QF-349 ticket - https://skillsfundingagency.atlassian.net/browse/QF-349
                // Raised to potentially query apprentice commitments / account api 
                // For now we should return an error to allow the UX to show a relevant page.

                //Should we return specific error information here?
                return new GetApprenticeTrainingProvidersResult()
                {
                    TrainingProviders = new List<TrainingProvider>()
                };
            }

            // 2. Filter out statuses we don't need to process.
            //var validStatuses = new[] { ApprenticeFeedbackTarget.FeedbackTargetStatus.Active, ApprenticeFeedbackTarget.FeedbackTargetStatus.NotYetActive, ApprenticeFeedbackTarget.FeedbackTargetStatus.Complete };
            //apprenticeFeedbackTargets = apprenticeFeedbackTargets.Where(s => validStatuses.Contains(s.Status));

            // 3. Setup Learner aggregate object holder to contain information from external systems to supply to Inner Api.
            var learnerAggregate = new List<ApprenticeLearnerAggregate>();

            // 4. Get corresponding Learner Record for apprentice feedback target record and update aggregate if found.
            _logger.LogDebug($"Processing feedback targets for: {request.ApprenticeId}");
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
                    FeedbackTarget = feedbackTarget,
                    Learner = learnerResponse.Body
                });
            }

            // 5. Get Confidentiality count, which providers have enough active apprentices to allow feedback to occur.
            var ukprns = apprenticeFeedbackTargets.GroupBy(s => s.Ukprn).Select(s => s.Key).ToList();
            _logger.LogDebug($"Processing UKPRNs to retrieve active learners for ApprenticeId: {request.ApprenticeId}");
            foreach (var ukprn in ukprns)
            {
                // Retrieve latest count from aprentice commitments api for the given provider.
                _logger.LogDebug($"Processing UKPRN to retrieve active learners count UKPRN: {ukprn}");
                //var activeLearnersResponse = await _apprenticeCommitmentsApiClient.GetWithResponseCode<int>(new GetActiveApprenticeLearnerCountRequest());

                //if (activeLearnersResponse.StatusCode != System.Net.HttpStatusCode.OK)
                //{
                //    _logger.LogError($"Error Retrieving active apprenticeships count for Ukprn: {ukprn}, Content: {activeLearnersResponse.ErrorContent}");
                //    continue;
                //}

                //learnerAggregate.Where(s => s.FeedbackTarget.Ukprn == ukprn).ToList().ForEach(s => s.LearnerCountForProvider = activeLearnersResponse.Body);
                
                // Temporary hardcode of active learner count to bypass this api call as it may not even be needed
                learnerAggregate.Where(s => s.FeedbackTarget.Ukprn == ukprn).ToList().ForEach(s => s.LearnerCountForProvider = 99);
            }

            // 6. Send Update Call to Inner Api with latest information to process and update the feedback target.
            var updatedTargets = new List<ApprenticeFeedbackTarget>();
            _logger.LogDebug($"Processing aggregated learners to update the feedback targets for ApprenticeId: {request.ApprenticeId}");
            foreach (var aggregate in learnerAggregate)
            {
                _logger.LogDebug($"Updating Feedback Target with Id: {aggregate.FeedbackTarget.Id} for ApprenticeId: {request.ApprenticeId}");
                var updateApprenticeFeedbackTargetRequest = new UpdateApprenticeFeedbackTargetRequest(
                    new UpdateApprenticeFeedbackTargetRequestData
                    {
                        ApprenticeFeedbackTargetId = aggregate.FeedbackTarget.Id,
                        ActiveApprenticeshipsCount = aggregate.LearnerCountForProvider,
                        Learner = (LearnerData)aggregate.Learner,
                    });

                var response = await _apprenticeFeedbackApiClient.PostWithResponseCode<ApprenticeFeedbackTarget>(updateApprenticeFeedbackTargetRequest);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"Error Updating the apprentice feedback target with Id: {aggregate.FeedbackTarget.Id} for apprenticeId: {request.ApprenticeId}, Content: {response.ErrorContent}");
                    continue;
                }

                aggregate.FeedbackTarget = response.Body;
            }

            // 7. Transform from the latest aggregate 
            // If there is more than one apprenticeship for one Ukprn then the latest valid apprenticeship should be taken
            // As part of the return values back from Apim.

            // Api call to inner feedback api, get me providers that can be fed back on
            // for the apprentice

            // Passes in the Apprentice Guid

            var trainingProviders = await _apprenticeFeedbackApiClient.
              GetAll<TrainingProvider>(new GetAllTrainingProvidersForApprenticeRequest { ApprenticeId = request.ApprenticeId });

            _logger.LogDebug($"End GetApprenticeshipTrainingProviderQueryHandler for ApprenticeId:{request.ApprenticeId}");
            return new GetApprenticeTrainingProvidersResult
            {
                TrainingProviders = trainingProviders
            };
        }
    }
}
