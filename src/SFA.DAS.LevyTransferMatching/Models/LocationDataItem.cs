namespace SFA.DAS.LevyTransferMatching.Models
{
    public class LocationDataItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double[] GeoPoint { get; set; }
        public string LocalAuthorityName { get; set; }
        public string LocalAuthorityDistrict { get; set; }
        public string County { get; set; }
        public string Region { get; set; }
    }
}
