using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Interfaces;

public interface INhsJobsApiClient
{
    Task<ApiResponse<string>> GetWithResponseCode(IGetApiRequest request);
}