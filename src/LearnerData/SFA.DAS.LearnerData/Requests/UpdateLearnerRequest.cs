namespace SFA.DAS.LearnerData.Requests;

public class UpdateLearnerRequest
{
    public UpdateLearnerRequestDeliveryDetails Delivery { get; set; }
}
public class UpdateLearnerRequestDeliveryDetails
{
    public DateTime? CompletionDate { get; set; }
}