namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetAddressesListItem
    {
        public string Uprn { get; set; }
        public string House { get; set; }
        public string Street { get; set; }
        public string Locality { get; set; }
        public string PostTown { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Match { get; set; }
        public string Organisation { get; set; }
    }
}
