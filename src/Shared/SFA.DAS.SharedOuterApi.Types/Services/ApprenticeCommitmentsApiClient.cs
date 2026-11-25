using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace SFA.DAS.SharedOuterApi.Types.Services;

[ExcludeFromCodeCoverage]
public class ApprenticeCommitmentsApiClient(IInternalApiClient<ApprenticeCommitmentsApiConfiguration> apiClient)
    : IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>
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

    public async Task<HttpStatusCode> Patch<TRequest>(
        IPatchApiRequest<TRequest> request)
    {
        await apiClient.Patch(request);
        return HttpStatusCode.OK;
    }

    public Task<ApiResponse<TResponse>> PatchWithResponseCode<TRequest, TResponse>(
        IPatchApiRequest<TRequest> request)
    {
        return apiClient.PatchWithResponseCode<TRequest, TResponse>(request);
    }

    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = false)
    {
        return apiClient.PostWithResponseCode<TResponse>(request);
    }
}