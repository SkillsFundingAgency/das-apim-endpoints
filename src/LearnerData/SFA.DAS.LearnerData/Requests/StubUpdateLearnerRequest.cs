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
    public string Ukprn { get; set; }
    public string TrainingType { get; set; }
    public string ConsumerReference { get; set; }
}

public class StubLearner
{
    public string Uln { get; set; }
    public string LearnerRef { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public DateTime? Dob { get; set; }
    public string Email { get; set; }
    public bool HasEhcp { get; set; }
}

public class StubDelivery
{
    public StubOnProgramme OnProgramme { get; set; }
    public List<StubEnglishAndMaths> EnglishAndMaths { get; set; }
}

public class StubOnProgramme
{
    public StubCare Care { get; set; }
    public int StandardCode { get; set; }
    public string AgreementId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ExpectedEndDate { get; set; }
    public int OffTheJobHours { get; set; }
    public int PercentageOfTrainingLeft { get; set; }
    public List<StubCost> Costs { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public List<StubLearningSupport> LearningSupport { get; set; }
}

public class StubCare
{
    public bool Careleaver { get; set; }
    public bool EmployerConsent { get; set; }
}

public class StubCost
{
    public int TrainingPrice { get; set; }
    public int EpaoPrice { get; set; }
    public DateTime FromDate { get; set; }
}

public class StubLearningSupport
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class StubEnglishAndMaths
{
    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public int CourseCode { get; set; }
    public int? PriorLearningPercentage { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }

    public List<StubLearningSupport> LearningSupport { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
