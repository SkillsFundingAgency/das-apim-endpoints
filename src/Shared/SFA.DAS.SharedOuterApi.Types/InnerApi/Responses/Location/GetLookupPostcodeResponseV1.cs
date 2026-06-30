using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

public record GetLookupPostcodeResponseV1
{
    [JsonPropertyName("postcode")]
    public string Postcode { get; set; }

    [JsonPropertyName("outcode")]
    public string Outcode { get; set; }

    [JsonPropertyName("incode")]
    public string Incode { get; set; }

    [JsonPropertyName("districtName")]
    public string DistrictName { get; set; }

    [JsonPropertyName("location")]
    public Location Location { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("localAuthorityName")]
    public string LocalAuthorityName { get; set; }
}

public record Location
{
    [JsonPropertyName("coordinates")]
    public List<double> Coordinates { get; set; } = [];
}