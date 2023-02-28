using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Learners;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Learners;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.ValidateDraftLearnerDetails.Queries
{
    public partial class ValidateDraftLearnerDetailsQueryHandler : IRequestHandler<ValidateDraftLearnerDetailsQuery, ValidateDraftLearnerDetailsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsApiClient;
        private readonly ILearnerValidationService _learnerValidationService;

        public ValidateDraftLearnerDetailsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient, ILearnerValidationService learnerValidationService)
        {
            _commitmentsApiClient = commitmentsApiClient;
            _learnerValidationService = learnerValidationService;
        }

        public async Task<ValidateDraftLearnerDetailsQueryResult> Handle(ValidateDraftLearnerDetailsQuery request, CancellationToken cancellationToken)
        {
            var learnerValidationResponse = await _learnerValidationService.ValidateLearner(request.Uln, request.FirstName, request.LastName);

            var verifyLearnerValidationResponseQueryRequestBody = new VerifyLearnerValidationResponseQueryRequest.Body()
            {
                SearchedULN = learnerValidationResponse.SearchedULN,
                ResponseCode = learnerValidationResponse.ResponseCode,
                Uln = request.Uln
            };
            var verifyLearnerValidationResponseQueryRequest = new VerifyLearnerValidationResponseQueryRequest(verifyLearnerValidationResponseQueryRequestBody);
            var response = await _commitmentsApiClient.PostWithResponseCode<VerifyLearnerValidationResponseQueryResponse>(verifyLearnerValidationResponseQueryRequest);
            response.EnsureSuccessStatusCode();

            return new ValidateDraftLearnerDetailsQueryResult
            {
                IsValid = response.Body.IsValid
            };
        }
    }
}