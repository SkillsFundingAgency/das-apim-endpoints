using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;

namespace SFA.DAS.SharedOuterApi.Models;

public class PostcodeInfo
{
    public string AdminDistrict { get; set; }
    public string Country { get; set; }
    public string Incode { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string Outcode { get; set; }
    public string Postcode { get; set; }

    public static PostcodeInfo From(GetLookupPostcodeResponse source)
    {
        return new PostcodeInfo
        {
            AdminDistrict = source.DistrictName,
            Country = source.Country,
            Incode = source.Incode,
            Latitude = source.Latitude,
            Longitude = source.Longitude,
            Outcode = source.Outcode,
            Postcode = source.Postcode,
        };
    }
}