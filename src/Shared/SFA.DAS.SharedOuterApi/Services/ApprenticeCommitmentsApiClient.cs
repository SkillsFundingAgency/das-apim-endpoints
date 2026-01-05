using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class ApprenticeCommitmentsApiClient : IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>
    {
        private readonly IInternalApiClient<ApprenticeCommitmentsApiConfiguration> _apiClient;

        public ApprenticeCommitmentsApiClient(IInternalApiClient<ApprenticeCommitmentsApiConfiguration> apiClient)
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

        public async Task<HttpStatusCode> Patch<TRequest>(
           IPatchApiRequest<TRequest> request)
        {
            await _apiClient.Patch(request);
            return HttpStatusCode.OK;
        }

        public Task<ApiResponse<TResponse>> PatchWithResponseCode<TRequest, TResponse>(
            IPatchApiRequest<TRequest> request)
        {
            return _apiClient.PatchWithResponseCode<TRequest, TResponse>(request);
        }
    }
}