using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum NotificationFrequency
{
    Default,
    Never,
    Immediately,
    Daily,
    Weekly,
}