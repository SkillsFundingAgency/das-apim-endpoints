using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services
{
    [ExcludeFromCodeCoverage]
    public class BusinessMetricsApiClient(IInternalApiClient<BusinessMetricsConfiguration> apiClient)
        : IBusinessMetricsApiClient<BusinessMetricsConfiguration>
    {
        public Task<TResponse> Get<TResponse>(IGetApiRequest request)
        {
            return apiClient.Get<TResponse>(request);
        }

        public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
        {
            return apiClient.GetResponseCode(request);
        }

        public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
        {
            return apiClient.GetWithResponseCode<TResponse>(request);
        }
    }
}