using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration;
public record CivilServiceJobsApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; }
    public string Identifier { get; set; }
}