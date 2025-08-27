using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum NotificationFrequency
{
    NotSet,
    Never,
    Immediately,
    Daily,
    Weekly,
}