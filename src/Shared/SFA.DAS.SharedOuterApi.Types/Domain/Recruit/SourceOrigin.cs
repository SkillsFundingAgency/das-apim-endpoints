using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.Domain.Recruit;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SourceOrigin
{
    Api,
    EmployerWeb,
    ProviderWeb,
    WebComplaint
}