namespace SFA.DAS.LearnerData.Requests;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class UpdateLearnerRequest
{
    public string ConsumerReference { get; set; }
    public UpdateLearnerRequestDeliveryDetails Delivery { get; set; }
}
public class UpdateLearnerRequestDeliveryDetails
{
    public OnProgrammeRequestDetails OnProgramme { get; set; }

    public List<MathsAndEnglish> EnglishAndMaths { get; set; }
}

public class OnProgrammeRequestDetails
{
    public DateTime? CompletionDate { get; set; }
    public List<LearningSupportRequestDetails> LearningSupport { get; set; }
}

public class MathsAndEnglish
{
    public string Course { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public int? PriorLearningPercentage { get; set; }
    public decimal Amount { get; set; }
    public List<LearningSupportRequestDetails> LearningSupport { get; set; }
}

public class LearningSupportRequestDetails
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.