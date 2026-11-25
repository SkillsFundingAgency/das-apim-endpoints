using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.LearnerData.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

namespace SFA.DAS.LearnerData.Services;

public interface ISldDataApiClient<T> : IGetApiClient<T>;

public class SldDataApiClient(IAccessTokenApiClient<SLDDataApiConfiguration> apiClient)
    : ISldDataApiClient<SLDDataApiConfiguration>
{
    public Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        return apiClient.Get<TResponse>(request);
    }

    public Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        return apiClient.GetWithResponseCode<TResponse>(request);
    }

    public Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        return apiClient.GetResponseCode(request);
    }
}
