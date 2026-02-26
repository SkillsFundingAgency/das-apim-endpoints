using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.LearnerData.Requests;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
[ExcludeFromCodeCoverage]
public class StubUpdateShortCourseRequest
{
    [Required]
    public string ConsumerReference { get; set; }
    [Required]
    public StubLearner Learner { get; set; }
    [Required]
    public StubShortCourseDelivery Delivery { get; set; }
}

[ExcludeFromCodeCoverage]
public class StubShortCourseDelivery
{
    [Required]
    public List<StubShortCourseOnProgramme> OnProgramme { get; set; }
}

[ExcludeFromCodeCoverage]
public class StubShortCourseOnProgramme
{
    [Required]
    public string? CourseCode { get; set; }
    public string? AgreementId { get; set; }
    [Required]
    public DateTime? StartDate { get; set; }
    [Required]
    public DateTime? ExpectedEndDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    [Required]
    public List<StubLearningSupport> LearningSupport { get; set; }
    [Required]
    public DateTime? PauseDate { get; set; }
    [Required]
    public int? AimSequenceNumber { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public StubMilestone[] Milestones { get; set; }
}

public enum StubMilestone
{
    ThirtyPercentLearningComplete,
    LearningComplete
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
