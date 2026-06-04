using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Shared;

namespace SFA.DAS.LearnerData.Requests;

#pragma warning disable CS8618
public class ShortCourseRequest
{
    public ShortCourseLearnerRequestDetails Learner { get; set; }

    public ShortCourseDelivery Delivery { get; set; }

    public string ConsumerReference { get; set; }
}

public class ShortCourseLearnerRequestDetails : LearnerRequestDetails
{
    public string LearnerRef { get; set; }
}

public class ShortCourseDelivery
{
    public List<ShortCourseOnProgramme> OnProgramme { get; set; }
}

public class ShortCourseOnProgramme
{
    public string CourseCode { get; set; }
    public string? AgreementId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public List<LearningSupport> LearningSupport { get; set; }
    public DateTime? PauseDate { get; set; }
    public int? AimSequenceNumber { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public Milestone[] Milestones { get; set; }
}


#pragma warning restore CS8618