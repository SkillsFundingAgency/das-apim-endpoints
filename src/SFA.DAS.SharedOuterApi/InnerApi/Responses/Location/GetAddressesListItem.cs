namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetAddressesListItem
    {
        public string Uprn { get; set; }
        public string Organisation { get; set; }
        public string Premises { get; set; }
        public string Thoroughfare { get; set; }
        public string Locality { get; set; }
        public string PostTown { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Match { get; set; }
    }
}
