using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipJobs.Interfaces;
public interface ICivilServiceJobsApiClient
{
    Task<ApiResponse<string>> GetWithResponseCode(IGetApiRequest request);
}