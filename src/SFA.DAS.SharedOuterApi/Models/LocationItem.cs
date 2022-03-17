namespace SFA.DAS.SharedOuterApi.Models
{
    public class LocationItem
    {
        public string Name { get ;}
        public double[] GeoPoint { get ;}
        public string Country { get ; set ; }
        public string LocalAuthorityName { get; set; }
        public string LocalAuthorityDistrict { get; set; }
        public string County { get; set; }
        public string Region { get; set; }

        public LocationItem (string name, double[] locationGeoPoint, string country, string localAuthorityName, string localAuthorityDistrict, string county, string region)
        {
            Name  = name;
            GeoPoint = locationGeoPoint;
            Country = country;
            LocalAuthorityName = localAuthorityName;
            LocalAuthorityDistrict = localAuthorityDistrict;
            County = county;
            Region = region;
        }
    }
}