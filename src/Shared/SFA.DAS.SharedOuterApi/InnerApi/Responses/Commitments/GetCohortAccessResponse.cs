using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;

public record GetCohortAccessResponse
{
    [JsonPropertyName(nameof(HasCohortAccess))]
    public bool HasCohortAccess { get; set; }
}