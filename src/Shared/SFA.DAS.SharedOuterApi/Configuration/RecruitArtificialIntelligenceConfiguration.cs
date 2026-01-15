using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Configuration;

public sealed record RecruitArtificialIntelligenceConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; }
    public string Identifier { get; set; }
}