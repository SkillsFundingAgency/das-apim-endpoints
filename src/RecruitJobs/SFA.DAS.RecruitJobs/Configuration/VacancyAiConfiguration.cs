using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RecruitJobs.Configuration;

[ExcludeFromCodeCoverage]
public class VacancyAiConfiguration
{
    public string LlmKey { get; set; }
    public string LlmEndpointShort { get; set; }
    public AiPrompt DiscriminationPrompt { get; set; }
    public AiPrompt MissingContentPrompt { get; set; }
    public AiPrompt SpellingCheckPrompt { get; set; }
}

[ExcludeFromCodeCoverage]
public class AiPrompt
{
    public string SystemPrompt { get; set; }
    public string UserHeader { get; set; }
    public string UserInstruction { get; set; }
}