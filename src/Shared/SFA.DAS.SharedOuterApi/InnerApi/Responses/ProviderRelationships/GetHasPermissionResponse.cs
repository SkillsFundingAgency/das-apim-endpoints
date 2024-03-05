using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;

public sealed class GetHasPermissionResponse
{
    [JsonPropertyName(nameof(HasPermission))]
    public bool HasPermission { get; set; }
}