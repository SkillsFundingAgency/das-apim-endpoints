using System.Text.Json.Serialization;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;

public class GetLookupPostcodeResponse
{
    [JsonPropertyName("postcode")]
    public string Postcode { get; set; }
    
    [JsonPropertyName("outcode")]
    public string Outcode { get; set; }
    
    [JsonPropertyName("incode")]
    public string Incode { get; set; }
    
    [JsonPropertyName("districtName")]
    public string DistrictName { get; set; }
    
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }
    
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
    
    [JsonPropertyName("country")]
    public string Country { get ; set ; }
}