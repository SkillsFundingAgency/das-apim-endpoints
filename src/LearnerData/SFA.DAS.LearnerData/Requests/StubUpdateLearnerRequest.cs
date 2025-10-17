using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.LearnerData.Requests;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
[ExcludeFromCodeCoverage]
public class StubUpdateLearnerRequest
{
    [Required]
    public string ConsumerReference { get; set; }
    [Required]
    public StubLearner Learner { get; set; }
    [Required]
    public StubDelivery Delivery { get; set; }
}

[ExcludeFromCodeCoverage]
public class StubLearner
{
    [Required]
    public string Uln { get; set; }
    [Required]
    public string LearnerRef { get; set; }
    [Required]
    public string Firstname { get; set; }
    [Required]
    public string Lastname { get; set; }
    [Required]
    public DateTime? Dob { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public bool? HasEhcp { get; set; }
}

[ExcludeFromCodeCoverage]
public class StubDelivery
{
    [Required]
    public StubOnProgramme OnProgramme { get; set; }
    [Required]
    public List<StubEnglishAndMaths> EnglishAndMaths { get; set; }
}

[ExcludeFromCodeCoverage]
public class StubOnProgramme
{
    [Required]
    public StubCare Care { get; set; }
    [Required]
    public int? StandardCode { get; set; }
    public string? AgreementId { get; set; }
    [Required]
    public DateTime? StartDate { get; set; }
    [Required]
    public DateTime? ExpectedEndDate { get; set; }
    [Required]
    public int? OffTheJobHours { get; set; }
    [Required]
    public List<StubCost> Costs { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    [Required]
    public List<StubLearningSupport> LearningSupport { get; set; }
    [Required]
    public bool? IsFlexiJob { get; set; }
    public DateTime? PauseDate { get; set; }
    [Required]
    public int? AimSequenceNumber { get; set; }
    [Required]
    public string LearnerAimRef { get; set; }
}

[ExcludeFromCodeCoverage]
public class StubCare
{
    [Required]
    public bool? Careleaver { get; set; }
    [Required]
    public bool? EmployerConsent { get; set; }
}

[ExcludeFromCodeCoverage]
public class StubCost
{
    public int? TrainingPrice { get; set; }
    public int? EpaoPrice { get; set; }
    public DateTime? FromDate { get; set; }
}

[ExcludeFromCodeCoverage]
public class StubLearningSupport
{
    [Required]
    public DateTime? StartDate { get; set; }
    [Required]
    public DateTime? EndDate { get; set; }
}

[ExcludeFromCodeCoverage]
public class StubEnglishAndMaths
{
    [Required]
    public DateTime? StartDate { get; set; }
    [Required]
    public DateTime? EndDate { get; set; }
    [Required]
    public int? CourseCode { get; set; }
    public int? PriorLearningAdjustment { get; set; }
    public int? OtherFundingAdjustment { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }

    public List<StubLearningSupport> LearningSupport { get; set; }
    [Required]
    public int? AimSequenceNumber { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
