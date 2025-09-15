using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

public class SaveCompletionApiPatchRequest : IPatchApiRequest<SaveCompletionRequest>
{
    public string PatchUrl { get; }

    public SaveCompletionRequest Data { get; set; }

    public SaveCompletionApiPatchRequest(Guid learningKey, SaveCompletionRequest data)
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

public class SaveLearningSupportApiPutRequest : IPatchApiRequest<SaveLearningSupportRequest>
{
    public string PatchUrl { get; }

    public SaveLearningSupportRequest Data { get; set; }

    public SaveLearningSupportApiPutRequest(Guid learningKey, SaveLearningSupportRequest data)
    {
        PatchUrl = $"apprenticeship/{learningKey.ToString()}/learningSupport";
        Data = data;
    }
}

public class SaveLearningSupportRequest : List<LearningSupportPaymentDetail> { }

public class LearningSupportPaymentDetail
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class SavePricesApiPatchRequest : IPatchApiRequest<SavePricesRequest>
{
    public string PatchUrl { get; }

    public SavePricesRequest Data { get; set; }

    public SavePricesApiPatchRequest(Guid apprenticeshipKey, SavePricesRequest data)
    {
        PatchUrl = $"apprenticeship/{apprenticeshipKey}/prices";
        Data = data;
    }
}

public class SavePricesRequest
{
    public Guid ApprenticeshipEpisodeKey { get; set; }
    public List<PriceDetail> Prices { get; set; } = [];
}

public class PriceDetail
{
    public Guid Key { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TrainingPrice { get; set; }
    public decimal? EndPointAssessmentPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public int FundingBandMaximum { get; set; }
}