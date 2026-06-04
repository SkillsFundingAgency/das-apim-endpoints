using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitQa.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmployerNameOption
{
    RegisteredName,
    TradingName,
    Anonymous
}