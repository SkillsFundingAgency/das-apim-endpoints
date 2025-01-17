using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services
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
