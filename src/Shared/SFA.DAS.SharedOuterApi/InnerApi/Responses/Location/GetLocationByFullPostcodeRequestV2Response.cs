namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;

public class GetLocationByFullPostcodeRequestV2Response
{
    public string Postcode { get; set; }
    public string Outcode { get; set; }
    public string Incode { get; set; }
    public string DistrictName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string Country { get ; set ; }

    public GetLocationsListItem ToGetLocationsListItem()
    {
        var result = new GetLocationsListItem
        {
            Postcode = Postcode,
            Country = Country,
            DistrictName = DistrictName,
        };

        if (Latitude is not null && Longitude is not null)
        {
            result.Location = new GetLocationsListItem.Coordinates()
            {
                GeoPoint = [Latitude.Value, Longitude.Value],
            };
        }

        return result;
    }
}