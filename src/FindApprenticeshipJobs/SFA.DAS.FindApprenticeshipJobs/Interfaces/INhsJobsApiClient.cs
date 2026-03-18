using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Interfaces;

public interface INhsJobsApiClient
{
    Task<ApiResponse<string>> GetWithResponseCode(IGetApiRequest request);
}