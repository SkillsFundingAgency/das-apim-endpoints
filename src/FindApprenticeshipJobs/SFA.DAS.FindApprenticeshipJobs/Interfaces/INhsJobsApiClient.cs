using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Interfaces;

public interface INhsJobsApiClient<T> : IGetApiClient<NhsJobsConfiguration>
{
    
}