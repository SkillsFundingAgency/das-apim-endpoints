namespace SFA.DAS.SharedOuterApi.Types.Models;

public class FeedbackTransactionClick
{
    public Guid ApprenticeFeedbackTargetId { get; set; }
    public string LinkName { get; set; }
    public string LinkUrl { get; set; }
    public DateTime ClickedOn { get; set; }
}