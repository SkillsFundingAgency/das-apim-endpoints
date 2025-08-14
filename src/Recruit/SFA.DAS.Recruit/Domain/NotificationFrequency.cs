using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationFrequency
{
    Default,
    Never,
    Immediately,
    Daily,
    Weekly,
}