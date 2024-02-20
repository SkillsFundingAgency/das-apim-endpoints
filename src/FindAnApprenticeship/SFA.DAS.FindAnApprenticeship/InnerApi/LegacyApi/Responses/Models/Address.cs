namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Models
{
    public record Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string Postcode { get; set; }
        public string Uprn { get; set; }
        public GeoPoint GeoPoint { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
    }
}
