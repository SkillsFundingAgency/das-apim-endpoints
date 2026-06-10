using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RuleId
{
    ProfanityChecks,
    BannedPhraseChecks,
    VacancyAnonymous
}