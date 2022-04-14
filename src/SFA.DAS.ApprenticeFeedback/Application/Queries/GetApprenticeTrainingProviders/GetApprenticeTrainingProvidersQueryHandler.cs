using MediatR;
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

        public GetApprenticeTrainingProvidersQueryHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _assessorsApiClient = assessorsApiClient;
        }

        public async Task<GetApprenticeTrainingProvidersResult> Handle(GetApprenticeTrainingProvidersQuery request, CancellationToken cancellationToken)
        {
            var apprenticeFeedbackTargets = await _apprenticeFeedbackApiClient.
                GetAll<ApprenticeFeedbackTarget>(new GetAllApprenticeFeedbackTargetsRequest { ApprenticeId = request.ApprenticeId });

            if (apprenticeFeedbackTargets?.Count() == 0)
            {
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

            var validStatuses = new[] { ApprenticeFeedbackTarget.FeedbackTargetStatus.Active, ApprenticeFeedbackTarget.FeedbackTargetStatus.NotYetActive };
            apprenticeFeedbackTargets = apprenticeFeedbackTargets.Where(s => validStatuses.Contains(s.Status));
            
            foreach(var feedbackTarget in apprenticeFeedbackTargets)
            {
                var learner = await _assessorsApiClient.Get<GetApprenticeLearnerResponse>(new GetApprenticeLearnerRequest(feedbackTarget.ApprenticeshipId));
                // if learner is null, say so on the apprentice inner api update request.
            }

            var ukprns = apprenticeFeedbackTargets.GroupBy(s => s.Ukprn).Select(s => s.Key).ToList();

            foreach (var ukprn in ukprns)
            {
                // Retrieve latest count from aprentice commitments api for the given provider and standard
            }

            // Send Update Call to Inner Api with latest information to update the feedback target

            var trainingProviders = new List<TrainingProvider>();

            return new GetApprenticeTrainingProvidersResult
            {
                TrainingProviders = trainingProviders
            };
        }
    }
}
