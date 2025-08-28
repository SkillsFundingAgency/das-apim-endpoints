using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LearnerData.Requests;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class StubUpdateLearnerRequest
{
    [Required]
    public StubHeader Header { get; set; }
    [Required]
    public StubLearner Learner { get; set; }
    [Required]
    public List<StubDelivery> Delivery { get; set; }

}

public class StubHeader
{
    [Required]
    public string Ukprn { get; set; }
    [Required]
    public string LearningType { get; set; }
    [Required]
    public string ConsumerReference { get; set; }
}

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

public class StubDelivery
{
    [Required]
    public StubOnProgramme OnProgramme { get; set; }
    [Required]
    public List<StubEnglishAndMaths> EnglishAndMaths { get; set; }
}

public class StubOnProgramme
{
    [Required]
    public StubCare Care { get; set; }
    [Required]
    public int? StandardCode { get; set; }
    [Required]
    public string AgreementId { get; set; }
    [Required]
    public DateTime? StartDate { get; set; }
    [Required]
    public DateTime? ExpectedEndDate { get; set; }
    [Required]
    public int? OffTheJobHours { get; set; }
    [Required]
    public int? PercentageOfTrainingLeft { get; set; }
    [Required]
    public List<StubCost> Costs { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    [Required]
    public List<StubLearningSupport> LearningSupport { get; set; }
}

public class StubCare
{
    [Required]
    public bool? Careleaver { get; set; }
    [Required]
    public bool? EmployerConsent { get; set; }
}

public class StubCost
{
    public int TrainingPrice { get; set; }
    public int EpaoPrice { get; set; }
    [Required]
    public DateTime? FromDate { get; set; }
}

public class StubLearningSupport
{
    [Required]
    public DateTime? StartDate { get; set; }
    [Required]
    public DateTime? EndDate { get; set; }
}

public class StubEnglishAndMaths
{
    [Required]
    public DateTime? StartDate { get; set; }
    [Required]
    public DateTime? PlannedEndDate { get; set; }
    [Required]
    public int? CourseCode { get; set; }
    [Required]
    public int? PriorLearningPercentage { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }

    public List<StubLearningSupport> LearningSupport { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
