using System.Net;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Interfaces;

namespace SFA.DAS.Vacancies.Services
{
    public class ProviderRelationshipsApiClient : IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>
    {
        private readonly IGetApiClient<ProviderRelationshipsApiConfiguration> _apiClient;

        public ProviderRelationshipsApiClient (IGetApiClient<ProviderRelationshipsApiConfiguration> apiClient)
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