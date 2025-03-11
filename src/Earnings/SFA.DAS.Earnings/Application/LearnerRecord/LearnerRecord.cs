namespace SFA.DAS.Earnings.Application.LearnerRecord;

public class LearnerRecord
{
    public long Uln { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string LearnerEmail { get; set; }
    public long Ukprn { get; set; }
    public int PriorLearningPercent { get; set; }
    public int StandardCode { get; set; }
    public int TrainingPrice { get; set; }
    public int EpaoPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public int AgreementId { get; set; }
    public bool IsFlexiJob { get; set; }
    public int PlannedOTJTrainingHours { get; set; }
}