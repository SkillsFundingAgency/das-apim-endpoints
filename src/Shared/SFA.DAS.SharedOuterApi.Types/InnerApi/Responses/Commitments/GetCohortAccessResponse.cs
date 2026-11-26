using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Commitments;

public record GetCohortAccessResponse
{
    [JsonPropertyName(nameof(HasCohortAccess))]
    public bool HasCohortAccess { get; set; }
}