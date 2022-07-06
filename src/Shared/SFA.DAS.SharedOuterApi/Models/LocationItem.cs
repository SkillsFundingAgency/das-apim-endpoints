namespace SFA.DAS.SharedOuterApi.Models
{
    public class LocationItem
    {
        public string Name { get ;}
        public double[] GeoPoint { get ;}
        public string Country { get ; set ; }

        public LocationItem (string name, double[] locationGeoPoint, string country)
        {
            Name  = name;
            GeoPoint = locationGeoPoint;
            Country = country;
        }
    }
}