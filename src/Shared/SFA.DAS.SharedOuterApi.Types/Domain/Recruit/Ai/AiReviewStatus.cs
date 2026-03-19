using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.Domain.Recruit.Ai;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AiReviewStatus
{
    Pending,
    Passed,
    Failed,
}