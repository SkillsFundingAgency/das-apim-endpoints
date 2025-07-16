using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Configuration;
public class CivilServiceJobsConfiguration : IApiConfiguration
{
    public required string Url { get; set; }
    public required string ApiKey { get; set; }
}