using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.Configuration;

public sealed record RecruitAiApiConfiguration : IInternalApiConfiguration
{
    public string Url { get; set; }
    public string Identifier { get; set; }
}