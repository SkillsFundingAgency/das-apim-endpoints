using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.Domain.Domain.Recruit;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmployerNameOption
{
    RegisteredName,
    TradingName,
    Anonymous
}