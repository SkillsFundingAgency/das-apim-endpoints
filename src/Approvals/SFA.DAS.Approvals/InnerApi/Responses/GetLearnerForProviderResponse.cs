using System;

namespace SFA.DAS.Approvals.InnerApi.Responses;

public record GetLearnerForProviderResponse
{
    public long Uln { get; set; }
    public long Ukprn { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime Dob { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public int EpaoPrice { get; set; }
    public int TrainingPrice { get; set; }
    public int StandardCode { get; set; }
    public bool IsFlexiJob { get; set; }
    public int PlannedOTJTrainingHours { get; set; }

}