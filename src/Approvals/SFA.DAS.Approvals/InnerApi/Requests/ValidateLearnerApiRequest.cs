using SFA.DAS.Approvals.Types;
using System;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class ValidateLearnerApiRequest : SaveDataRequest
{
    public long ProviderId { get; set; }
    public long LearnerDataId { get; set; }
    public LearnerDataEnhanced Learner { get; set; }
}

public record LearnerDataEnhanced
{
    public long Uln { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime Dob { get; set; }
    public int EpaoPrice { get; set; }
    public int TrainingPrice { get; set; }
    public int Cost { get; set; }
    public string CourseCode { get; set; }
    public string StandardsCode { get; set; }
    //public string ProviderReference { get; set; }
    public DeliveryModel DeliveryModel { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ActualStartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public int? MinimumAgeAtApprenticeshipStart { get; set; }
    public int? MaximumAgeAtApprenticeshipStart { get; set; }
}