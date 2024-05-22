using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Authorization;

public record GetCohortAccessResponse
{
    [JsonPropertyName(nameof(HasCohortAccess))]
    public bool HasCohortAccess { get; set; }
}
