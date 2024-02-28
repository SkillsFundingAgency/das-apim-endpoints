using SFA.DAS.FindApprenticeshipJobs.Configuration;
using SFA.DAS.FindApprenticeshipJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.FindApprenticeshipJobs.Services;

public class NhsJobsApiClient : GetApiClient<NhsJobsConfiguration>, INhsJobsApiClient<NhsJobsConfiguration>
{
    public NhsJobsApiClient(IHttpClientFactory httpClientFactory, NhsJobsConfiguration apiConfiguration) : base(httpClientFactory, apiConfiguration)
    {
    }
    
    protected override async Task AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
    }
}