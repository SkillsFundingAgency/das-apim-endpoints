using System.Collections.Generic;
using System.Text.Json.Serialization;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;

public class GetBulkPostcodeDataResponse
{
    [JsonPropertyName("locations")]
    public List<PostcodeDataResponse> Results { get; set; }
}

public class PostcodeDataResponse
{
    public string Postcode { get; set; }
    public string Country { get; set; }
    public LocationResponse Location { get; set; }

    public PostcodeData ToDomain()
    {
        return new PostcodeData(Postcode, Country, Location.Coordinates[0], Location.Coordinates[1]);
    }
}

public class LocationResponse
{
    public double[] Coordinates { get; set; }
}