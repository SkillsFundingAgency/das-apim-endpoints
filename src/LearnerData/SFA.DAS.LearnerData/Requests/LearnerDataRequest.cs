using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LearnerData.Requests;

public class LearnerDataRequest : IValidatableObject
{
    public long ULN { get; set; }
    public long UKPRN { get; set; }

    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime DoB { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime PlannedEndDate { get; set; }
    
    public int? PercentageLearningToBeDelivered { get; set; }

    public int EpaoPrice { get; set; }

    public int TrainingPrice { get; set; }
    public string? AgreementId { get; set; }
    public bool IsFlexiJob { get; set; }
    public int? PlannedOTJTrainingHours { get; set; }

    public int StandardCode { get; set; }

    [StringLength(100)]
    public string ConsumerReference { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ULN <= 1000000000 || ULN >= 9999999999)
            yield return new ValidationResult($"Learner data contains incorrect ULN {ULN}", new List<string> { "ULN" });

        if (UKPRN < 10000000 || UKPRN > 99999999)
            yield return new ValidationResult($"Learner data contains incorrect UKPRN {UKPRN}", new List<string> { "UKPRN" });

        if (EpaoPrice < 0)
            yield return new ValidationResult($"Learner data contains a negative EpaoPrice {EpaoPrice}", new List<string> {"EpaoPrice"});

        if (TrainingPrice < 0)
            yield return new ValidationResult($"Learner data contains a negative TrainingPrice {TrainingPrice}", new List<string> { "TrainingPrice" });

        if (PlannedOTJTrainingHours < 0)
            yield return new ValidationResult($"Learner data contains a negative PlannedOTJTrainingHours {PlannedOTJTrainingHours}", new List<string> { "PlannedOTJTrainingHours" });

        if (StandardCode < 0)
            yield return new ValidationResult($"Learner data contains a negative StandardCode {StandardCode}", new List<string> { "StandardCode" });
    }
}
