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
    public LearningUpdateDetails Learner { get; set; }
    public List<MathsAndEnglishDetails> MathsAndEnglishCourses { get; set; }
    public List<LearningSupportUpdatedDetails> LearningSupport { get; set; }
    public OnProgrammeDetails OnProgramme { get; set; }
}

public class LearningUpdateDetails
{
    public DateTime? CompletionDate { get; set; }
}

public class OnProgrammeDetails
{
    public DateTime ExpectedEndDate { get; set; }
    public List<Cost> Costs { get; set; }
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
    public int? PriorLearningPercentage { get; set; }
    public decimal Amount { get; set; }
}

public class LearningSupportUpdatedDetails
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}