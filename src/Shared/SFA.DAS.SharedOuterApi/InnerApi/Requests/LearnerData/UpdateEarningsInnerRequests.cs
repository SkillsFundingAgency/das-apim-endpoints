using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

public class SaveCompletionApiPutRequest : IPatchApiRequest<SaveCompletionRequest>
{
    public string PatchUrl { get; }

    public SaveCompletionRequest Data { get; set; }

    public SaveCompletionApiPutRequest(Guid learningKey, SaveCompletionRequest data)
    {
        PatchUrl = $"apprenticeship/{learningKey.ToString()}/completion";
        Data = data;
    }
}

public class SaveCompletionRequest
{
    public DateTime? CompletionDate { get; set; }
}

public class SaveMathsAndEnglishApiPatchRequest : IPatchApiRequest<SaveMathsAndEnglishRequest>
{
    public string PatchUrl { get; }

    public SaveMathsAndEnglishRequest Data { get; set; }

    public SaveMathsAndEnglishApiPatchRequest(Guid apprenticeshipKey, SaveMathsAndEnglishRequest data)
    {
        PatchUrl = $"apprenticeship/{apprenticeshipKey}/mathsAndEnglish";
        Data = data;
    }
}

public class SaveMathsAndEnglishRequest : List<MathsAndEnglishRequestDetail> { }

public class MathsAndEnglishRequestDetail
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Course { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public int? PriorLearningAdjustmentPercentage { get; set; }
    public DateTime? ActualEndDate { get; set; }
}