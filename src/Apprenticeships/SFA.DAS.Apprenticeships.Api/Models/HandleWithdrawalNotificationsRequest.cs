namespace SFA.DAS.Apprenticeships.Api.Models;

public class HandleWithdrawalNotificationsRequest
{
    public DateTime LastDayOfLearning { get; set; }
    public string Reason { get; set; }
}