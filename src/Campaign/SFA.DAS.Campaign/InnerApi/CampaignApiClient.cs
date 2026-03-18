using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.InnerApi;

public class CampaignApiClient(IInternalApiClient<CampaignApiConfiguration> apiClient) : ICampaignApiClient
{
    public Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request)
    {
        return apiClient.PostWithResponseCode<TResponse>(request);
    }
}
