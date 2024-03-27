using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services
{
    [ExcludeFromCodeCoverage]
    public class TrainingProviderApiClient : ITrainingProviderApiClient<TrainingProviderConfiguration>
    {
        private readonly IInternalApiClient<TrainingProviderConfiguration> _apiClient;

        public TrainingProviderApiClient(IInternalApiClient<TrainingProviderConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _apiClient.Get<TResponse>(request);
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return _apiClient.GetResponseCode(request);
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return _apiClient.GetWithResponseCode<TResponse>(request);
        }
    }
}
