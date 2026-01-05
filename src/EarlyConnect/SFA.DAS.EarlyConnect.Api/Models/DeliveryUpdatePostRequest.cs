namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class DeliveryUpdatePostRequest
    {
        public string Source { get; set; }
        public List<int> Ids { get; set; }
    }
}
