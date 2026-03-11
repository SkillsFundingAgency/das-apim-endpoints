using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Interfaces;

public interface IPostApiClient
{
    Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request);
}
