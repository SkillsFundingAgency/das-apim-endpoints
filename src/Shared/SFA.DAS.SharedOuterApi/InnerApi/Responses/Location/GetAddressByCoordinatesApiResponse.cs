using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
public record GetAddressByCoordinatesApiResponse
{
    [JsonProperty("uprn")]
    public string Uprn { get; set; }

    [JsonProperty("buildingName")]
    public string BuildingName { get; set; }

    [JsonProperty("addressLine1")]
    public string AddressLine1 { get; set; }

    [JsonProperty("addressLine2")]
    public string AddressLine2 { get; set; }

    [JsonProperty("addressLine3")]
    public string AddressLine3 { get; set; }

    [JsonProperty("postcode")]
    public string Postcode { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }
}