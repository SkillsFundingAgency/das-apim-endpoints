using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace SFA.DAS.SharedOuterApi.Types.Services;

[ExcludeFromCodeCoverage]
public class TrainingProviderApiClient(IInternalApiClient<TrainingProviderConfiguration> apiClient)
    : ITrainingProviderApiClient<TrainingProviderConfiguration>
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