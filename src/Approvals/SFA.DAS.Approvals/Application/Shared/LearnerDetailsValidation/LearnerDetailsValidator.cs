using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.Shared.LearnerDetailsValidation
{
    public partial class LearnerDetailsValidator : ILearnerDetailsValidator
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsApiClient;
        private readonly ILearnerVerificationApiClient<LearnerVerificationApiConfiguration> _learnerVerificationApiClient;

        public LearnerDetailsValidator(
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
            ILearnerVerificationApiClient<LearnerVerificationApiConfiguration> learnerVerificationApiClient)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _learnerVerificationApiClient = learnerVerificationApiClient;
        }

        public async Task<LearnerVerificationResponse> Validate(ValidateLearnerDetailsRequest request)
        {
            var learnerValidationResponse = await _learnerVerificationApiClient.Get<LearnerVerificationResponse>(
                new GetVerifyLearnerRequest(request.Uln, request.FirstName, request.LastName));

            return learnerValidationResponse;
        }
    }
}