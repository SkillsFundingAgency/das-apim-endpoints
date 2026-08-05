using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Interfaces;

public interface IPostApiClient
{
    Task<ApiResponse<TResponse>> PostWithResponseCode<TResponse>(IPostApiRequest request);
}
