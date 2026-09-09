using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Authorization;

public record GetApprenticeshipAccessResponse
{
    [JsonPropertyName(nameof(HasApprenticeshipAccess))]
    public bool HasApprenticeshipAccess { get; set; }
}