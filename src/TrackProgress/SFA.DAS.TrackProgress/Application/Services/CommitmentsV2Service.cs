using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.TrackProgress.Application.Services
{
    public class CommitmentsV2Service
    {
        private readonly IInternalApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2Api;

        public CommitmentsV2Service(IInternalApiClient<CommitmentsV2ApiConfiguration> commitmentsV2Api) 
            => _commitmentsV2Api = commitmentsV2Api;

        public async Task<ApiResponse<GetProviderResponse>> GetProvider(long providerId)
        {
            var request = new GetProviderRequest(providerId);
            return await _commitmentsV2Api.GetWithResponseCode<GetProviderResponse>(request);
        }

        public async Task<ApiResponse<GetApprenticeshipsResponse>> GetApprenticeship(long providerId, long uln, DateTime startDate)
        {
            var request = new GetApprenticeshipsRequest(providerId, uln, startDate);
            return await _commitmentsV2Api.GetWithResponseCode<GetApprenticeshipsResponse>(request);
        }
    }
}
