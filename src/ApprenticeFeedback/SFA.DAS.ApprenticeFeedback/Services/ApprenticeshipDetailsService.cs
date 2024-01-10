using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeFeedback.Application.Commands.UpdateApprenticeFeedbackTarget;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ApprenticeAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Assessor;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Services
{
    public class ApprenticeshipDetailsService : IApprenticeshipDetailsService
    {
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apprenticeAccountsApiClient;
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly ILogger<UpdateApprenticeFeedbackTargetCommandHandler> _logger;

        public ApprenticeshipDetailsService(
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apprenticeAccountsApiClient,
            IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient, 
            ILogger<UpdateApprenticeFeedbackTargetCommandHandler> logger)
        {
            _apprenticeAccountsApiClient = apprenticeAccountsApiClient;
            _assessorsApiClient = assessorsApiClient;
            _logger = logger;
        }

        public async Task<(LearnerData LearnerData, MyApprenticeshipData MyApprenticeshipData)> Get(Guid apprenticeId, long apprenticeshipId)
        {
            var apprenticeDetails = (LearnerData: (LearnerData)null, MyApprenticeshipData: (MyApprenticeshipData)null);

            try
            {
                // does the learner response = null if the apprenticeship has been superceeded??
                _logger.LogDebug($"Retrieving learner record with apprentice commitments Id: {apprenticeshipId}");
                var learnerResponse = await _assessorsApiClient.GetWithResponseCode<GetApprenticeLearnerResponse>(new GetApprenticeLearnerRequest(apprenticeshipId));

                if (learnerResponse.StatusCode != System.Net.HttpStatusCode.OK && learnerResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    var errorMsg = $"Error retrieving learner record with apprentice commitments Id: {apprenticeshipId}";
                    if (!string.IsNullOrWhiteSpace(learnerResponse.ErrorContent))
                    {
                        errorMsg += $", Content: {learnerResponse.ErrorContent}";
                    }
                    throw new Exception(errorMsg);
                }

                apprenticeDetails.LearnerData = learnerResponse.Body;

                if (apprenticeDetails.LearnerData == null)
                {
                    _logger.LogDebug($"Retrieving my apprenticeship record with apprentice Id: {apprenticeId}");
                    var myApprenticeshipResponse = await _apprenticeAccountsApiClient.GetWithResponseCode<GetMyApprenticeshipResponse>(new GetMyApprenticeshipRequest(apprenticeId));
                    if (myApprenticeshipResponse.StatusCode != System.Net.HttpStatusCode.OK && myApprenticeshipResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
                    {
                        var errorMsg = $"Error retrieving my apprenticeship record with apprentice Id: {apprenticeId}";
                        if (!string.IsNullOrWhiteSpace(myApprenticeshipResponse.ErrorContent))
                        {
                            errorMsg += $", Content: {myApprenticeshipResponse.ErrorContent}";
                        }
                        throw new Exception(errorMsg);
                    }

                    apprenticeDetails.MyApprenticeshipData = myApprenticeshipResponse.Body;
                }
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
            }

            return apprenticeDetails;
        }
    }
}
