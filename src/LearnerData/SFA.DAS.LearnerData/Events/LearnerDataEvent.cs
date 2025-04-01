using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.LearnerData.Events;

public class LearnerDataEvent
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

    public static implicit operator LearnerDataEvent(LearnerDataRequest request) => new LearnerDataEvent
    {
        ULN = request.ULN,
        UKPRN = request.UKPRN,
        Firstname = request.Firstname,
        Lastname = request.Lastname,
        Email = request.Email,
        DoB = request.DoB,
        StartDate = request.StartDate,
        PlannedEndDate = request.PlannedEndDate,
        PercentageLearningToBeDelivered = request.PercentageLearningToBeDelivered,
        EpaoPrice = request.EpaoPrice,
        TrainingPrice = request.TrainingPrice,
        AgreementId = request.AgreementId,
        IsFlexJob = request.IsFlexJob,
        PlannedOTJTrainingHours = request.PlannedOTJTrainingHours,
        StandardCode = request.StandardCode
    };
}
