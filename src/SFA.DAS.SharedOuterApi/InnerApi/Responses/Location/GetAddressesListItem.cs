namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetAddressesListItem
    {
        public string Uprn { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Match { get; set; }
    }
}
