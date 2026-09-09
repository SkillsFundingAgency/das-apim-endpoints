using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitQa.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProhibitedContentType
{
    BannedPhrases,
    Profanity,
}