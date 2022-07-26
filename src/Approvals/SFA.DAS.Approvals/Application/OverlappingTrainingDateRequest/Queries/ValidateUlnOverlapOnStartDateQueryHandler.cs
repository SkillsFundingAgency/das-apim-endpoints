using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Queries
{
    public class ValidateUlnOverlapOnStartDateQueryHandler : IRequestHandler<ValidateUlnOverlapOnStartDateQuery, ValidateUlnOverlapOnStartDateQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public ValidateUlnOverlapOnStartDateQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ValidateUlnOverlapOnStartDateQueryResult> Handle(ValidateUlnOverlapOnStartDateQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetWithResponseCode<ValidateUlnOverlapOnStartDateResponse>(
             new ValidateUlnOverlapOnStartDateQueryRequest(request.ProviderId, request.Uln, request.StartDate, request.EndDate));

            response.EnsureSuccessStatusCode();

            return new ValidateUlnOverlapOnStartDateQueryResult
            { HasOverlapWithApprenticeshipId = response.Body.HasOverlapWithApprenticeshipId, 
                HasStartDateOverlap = response.Body.HasStartDateOverlap };
        }
    }
}
