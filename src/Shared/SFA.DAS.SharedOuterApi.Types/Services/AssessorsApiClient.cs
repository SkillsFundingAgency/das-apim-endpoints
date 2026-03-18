using System.Net;
using SFA.DAS.SharedOuterApi.Interfaces;

using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.SharedOuterApi.Types.Services
{
    public class AssessorsApiClient : IAssessorsApiClient<AssessorsApiConfiguration>
    {
        private readonly IInternalApiClient<AssessorsApiConfiguration> _apiClient;

        public AssessorsApiClient(IInternalApiClient<AssessorsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return _apiClient.Get<TResponse>(request);
        }

        public Task<IEnumerable<TResponse>> GetAll<TResponse>(IGetAllApiRequest request)
        {
            return _apiClient.GetAll<TResponse>(request);
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