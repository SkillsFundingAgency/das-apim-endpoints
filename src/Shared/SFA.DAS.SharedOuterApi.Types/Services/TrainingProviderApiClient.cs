using System.Diagnostics.CodeAnalysis;
using System.Net;


using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Types.Services
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
