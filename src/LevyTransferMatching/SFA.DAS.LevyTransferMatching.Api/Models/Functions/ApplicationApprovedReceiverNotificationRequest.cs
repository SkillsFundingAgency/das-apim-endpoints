namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions;

public class ApplicationApprovedReceiverNotificationRequest
{
    public int ApplicationId { get; set; }
    public int PledgeId { get; set; }
    public long ReceiverId { get; set; }
    public string EncodedAccountId { get; set; }
    public string EncodedApplicationId { get; set; }
    public string TransfersBaseUrl { get; set; }
    public string UnsubscribeUrl { get; set; }
}
