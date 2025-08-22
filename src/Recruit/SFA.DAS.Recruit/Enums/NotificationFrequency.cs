using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NotificationFrequency
{
    NotSet,
    Never,
    Immediately,
    Daily,
    Weekly,
}