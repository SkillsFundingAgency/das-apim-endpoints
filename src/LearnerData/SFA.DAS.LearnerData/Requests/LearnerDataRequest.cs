namespace SFA.DAS.LearnerData.Requests;

public class LearnerDataRequest
{
    public long ULN { get; set; }
    public long UKPRN { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public DateTime DoB { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public int PercentageLearningToBeDelivered { get; set; }
    public int EpaoPrice { get; set; }
    public int TrainingPrice { get; set; }
    public string AgreementId { get; set; }
    public bool IsFlexJob { get; set; }
    public int PlannedOTJTrainingHours { get; set; }
    public int StandardCode { get; set; }
}
