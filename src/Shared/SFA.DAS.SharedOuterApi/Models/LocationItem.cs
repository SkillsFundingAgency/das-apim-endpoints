namespace SFA.DAS.SharedOuterApi.Models;
public record LocationItem(string Name, double[] GeoPoint, string Country)
{
    public decimal? Latitude
    {
        get
        {
            if (GeoPoint?.Length > 0 && decimal.TryParse(GeoPoint[0].ToString(), out var latitude))
            {
                return latitude;
            }
            return null;
        }
    }

    public decimal? Longitude
    {
        get
        {
            if (GeoPoint?.Length > 1 && decimal.TryParse(GeoPoint[1].ToString(), out var longitude))
            {
                return longitude;
            }
            return null;
        }
    }
}