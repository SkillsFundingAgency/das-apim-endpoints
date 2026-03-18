using System.Net;
using SFA.DAS.SharedOuterApi.Interfaces;

using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Types.Services
{
    public class LearnerDataApiClient : ILearnerDataApiClient<LearnerDataApiConfiguration>
    {
        private readonly IAccessTokenApiClient<LearnerDataApiConfiguration> _apiClient;

        public LearnerDataApiClient(IAccessTokenApiClient<LearnerDataApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return await _apiClient.Get<TResponse>(request);
        }

        public async Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return await _apiClient.GetWithResponseCode<TResponse>(request);
        }

        public async Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return await _apiClient.GetResponseCode(request);
        }
    }
}
