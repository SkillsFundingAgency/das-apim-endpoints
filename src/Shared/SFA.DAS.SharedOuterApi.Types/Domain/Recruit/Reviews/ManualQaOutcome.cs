using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.Domain.Recruit.Reviews;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ManualQaOutcome
{
    Approved,
    Referred,
    Transferred,
    Blocked,
    Bypassed
}