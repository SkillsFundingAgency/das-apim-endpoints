namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class CouseLocationModel
    {
        public LocationType LocationType { get; set; }
        public bool BlockRelease { get; set; }
        public bool DayRelease { get; set; }
        public string RegionName { get; set; }
        public string SubRegionName { get; set; }
        public LocationAddress Address { get; set; }
        public decimal? ProviderLocationDistanceInMiles { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }

    public class LocationAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
    }

    public enum LocationType
    {
        Provider = 0,
        National = 1,
        Regional = 2
    }
}
