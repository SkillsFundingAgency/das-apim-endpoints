using System.Text.Json.Serialization;

namespace SFA.DAS.RecruitQa.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SourceOrigin
{
    Api,
    EmployerWeb,
    ProviderWeb,
    WebComplaint
}