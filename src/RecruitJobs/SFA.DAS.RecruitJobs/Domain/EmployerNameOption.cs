using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitJobs.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmployerNameOption
{
    RegisteredName,
    TradingName,
    Anonymous
}