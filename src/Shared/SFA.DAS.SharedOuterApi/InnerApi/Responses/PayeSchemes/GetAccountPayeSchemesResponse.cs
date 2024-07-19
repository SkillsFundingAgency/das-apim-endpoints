using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.PayeSchemes;

public record GetAccountPayeSchemesResponse
{
    [JsonProperty(nameof(Id))]
    public string Id { get; set; }
    [JsonProperty(nameof(Href))]
    public string Href { get; set; }
};