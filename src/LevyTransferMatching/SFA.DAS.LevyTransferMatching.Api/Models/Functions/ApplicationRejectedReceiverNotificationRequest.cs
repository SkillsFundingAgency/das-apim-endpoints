namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions
{
    public class ApplicationRejectedReceiverNotificationRequest
    {
        public int ApplicationId { get; set; }
        public int PledgeId { get; set; }
        public long ReceiverId { get; set; }
        public string BaseUrl { get; set; }
        public string EncodedApplicationId { get; set; }
    }
}
