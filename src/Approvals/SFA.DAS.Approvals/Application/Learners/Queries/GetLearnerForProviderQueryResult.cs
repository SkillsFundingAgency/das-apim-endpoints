using System;

namespace SFA.DAS.Approvals.Application.Learners.Queries;

public class GetLearnerForProviderQueryResult
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

    public static implicit operator GetLearnerForProviderQueryResult(InnerApi.Responses.GetLearnerForProviderResponse source)
    {
        return new GetLearnerForProviderQueryResult
        {
            Uln = source.Uln,
            Ukprn = source.Ukprn,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Email = source.Email,
            Dob = source.Dob,
            StartDate = source.StartDate,
            PlannedEndDate = source.PlannedEndDate,
            EpaoPrice = source.EpaoPrice,
            TrainingPrice = source.TrainingPrice,
            StandardCode = source.StandardCode,
            IsFlexiJob = source.IsFlexiJob,
            PlannedOTJTrainingHours = source.PlannedOTJTrainingHours,
        };
    }
}