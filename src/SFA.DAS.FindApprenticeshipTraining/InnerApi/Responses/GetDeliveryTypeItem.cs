namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetDeliveryTypeItem
    {
        public string DeliveryModes { get; set; }
        public decimal DistanceInMiles { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string County { get; set; }
        public bool National { get; set; }
    }
}