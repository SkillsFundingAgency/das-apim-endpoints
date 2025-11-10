using System.Text.Json.Serialization;

namespace SFA.DAS.Recruit.Domain.Vacancy;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SourceOrigin
{
    Api,
    EmployerWeb,
    ProviderWeb,
    WebComplaint
}