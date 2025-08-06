namespace SFA.DAS.LearnerData.Requests;

public class UpdateLearnerRequest
{
    public UpdateLearnerRequestDeliveryDetails Delivery { get; set; }
}
public class UpdateLearnerRequestDeliveryDetails
{
    public DateTime? CompletionDate { get; set; }

    public List<MathsAndEnglish> MathsAndEnglishCourses { get; set; }
}

public class MathsAndEnglish
{
    public string Course { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public int? PriorLearningPercentage { get; set; }
    public decimal Amount { get; set; }
}
