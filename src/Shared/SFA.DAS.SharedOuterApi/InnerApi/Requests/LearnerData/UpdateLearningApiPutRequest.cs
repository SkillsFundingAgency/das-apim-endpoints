using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

public class UpdateLearningApiPutRequest : IPutApiRequest<UpdateLearningRequestBody>
{
    public string PutUrl { get; }

    public UpdateLearningRequestBody Data { get; set; }

    public UpdateLearningApiPutRequest(Guid learningKey, UpdateLearningRequestBody data)
    {
        PutUrl = learningKey.ToString();
        Data = data;
    }
}

public class UpdateLearningRequestBody
{
    public Delivery Delivery { get; set; }
    public LearningUpdateDetails Learner { get; set; }
    public List<MathsAndEnglishDetails> MathsAndEnglishCourses { get; set; }
    public List<LearningSupportUpdatedDetails> LearningSupport { get; set; }
    public OnProgrammeDetails OnProgramme { get; set; }
}

public class LearningUpdateDetails
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? EmailAddress { get; set; }
    public DateTime? CompletionDate { get; set; }
}

public class OnProgrammeDetails
{
    public DateTime ExpectedEndDate { get; set; }
    public List<Cost> Costs { get; set; }
    public DateTime? PauseDate { get; set; }
    public List<BreakInLearning> BreaksInLearning { get; set; }
}

public class Cost
{
    public int TrainingPrice { get; set; }
    public int? EpaoPrice { get; set; }
    public DateTime FromDate { get; set; }
}

public class MathsAndEnglishDetails
{
    public string Course { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public DateTime? PauseDate { get; set; }
    public int? PriorLearningPercentage { get; set; }
    public decimal Amount { get; set; }
}

public class LearningSupportUpdatedDetails
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class Delivery
{
    public DateTime? WithdrawalDate { get; set; }
}

public class BreakInLearning
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}