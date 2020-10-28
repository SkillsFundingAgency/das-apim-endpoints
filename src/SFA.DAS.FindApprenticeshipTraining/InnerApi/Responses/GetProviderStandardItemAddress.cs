namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProviderStandardItemAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public decimal DistanceInMiles { get; set; }
    }
}