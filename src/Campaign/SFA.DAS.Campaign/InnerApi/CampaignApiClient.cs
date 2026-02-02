using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.InnerApi;

public class CampaignApiClient(IInternalApiClient<CampaignApiConfiguration> apiClient) : ICampaignApiClient<CampaignApiConfiguration>
{
    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request, bool includeResponse = true)
    {
        return apiClient.PostWithResponseCode<TResponse>(request);
    }
}
