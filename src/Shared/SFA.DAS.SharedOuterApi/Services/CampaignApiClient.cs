using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services;

public class CampaignApiClient(IInternalApiClient<CampaignApiConfiguration> apiClient) : ICampaignApiClient<CampaignApiConfiguration>
{
    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        return apiClient.PostWithResponseCode<TResponse>(request);
    }
}
