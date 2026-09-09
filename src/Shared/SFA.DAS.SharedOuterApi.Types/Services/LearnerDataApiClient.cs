using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

namespace SFA.DAS.SharedOuterApi.Types.Services;

public class LearnerDataApiClient(IAccessTokenApiClient<LearnerDataApiConfiguration> apiClient)
    : ILearnerDataApiClient<LearnerDataApiConfiguration>
{
    public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        return await apiClient.Get<TResponse>(request);
    }

    public async Task<ApiResponse<TResponse>> GetWithResponseCode<TResponse>(IGetApiRequest request)
    {
        return await apiClient.GetWithResponseCode<TResponse>(request);
    }

    public async Task<HttpStatusCode> GetResponseCode(IGetApiRequest request)
    {
        return await apiClient.GetResponseCode(request);
    }
}