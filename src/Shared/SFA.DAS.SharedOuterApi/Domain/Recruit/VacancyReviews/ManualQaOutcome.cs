using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Domain.Recruit.VacancyReviews;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ManualQaOutcome : byte
{
    Approved,
    Referred,
    Transferred,
    Blocked
}