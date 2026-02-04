namespace SFA.DAS.LearnerData.Events;

public class LearnerDataEvent
{
    public long ULN { get; set; }
    public long UKPRN { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public DateTime DoB { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public int? PercentageLearningToBeDelivered { get; set; }
    public int EpaoPrice { get; set; }
    public int TrainingPrice { get; set; }
    public string? AgreementId { get; set; }
    public bool IsFlexiJob { get; set; }
    public int PlannedOTJTrainingHours { get; set; }
    public int StandardCode { get; set; }
    public string LarsCode { get; set; }
    public Guid CorrelationId { get; set; }
    public DateTime ReceivedDate { get; set; }
    public int AcademicYear { get; set; }
    public string ConsumerReference { get; set; }

}
