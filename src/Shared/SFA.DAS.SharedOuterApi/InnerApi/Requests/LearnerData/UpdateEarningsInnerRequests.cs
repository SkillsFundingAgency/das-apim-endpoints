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
    public DateTime? PauseDate { get; set; }
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
    public int AgeAtStartOfLearning { get; set; }
    public int FundingBandMaximum { get; set; }
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
}

public class WithdrawApiPatchRequest : IPatchApiRequest<WithdrawRequest>
{
    public string PatchUrl { get; }

    public WithdrawRequest Data { get; set; }

    public WithdrawApiPatchRequest(Guid apprenticeshipKey, WithdrawRequest data)
    {
        PatchUrl = $"apprenticeship/{apprenticeshipKey}/withdraw";
        Data = data;
    }
}

public class WithdrawRequest
{
    public DateTime WithdrawalDate { get; set; }
}

public class ReverseWithdrawalRequest
{
}

public class ReverseWithdrawalApiPatchRequest(Guid apprenticeshipKey) : IPatchApiRequest<ReverseWithdrawalRequest>
{
    public string PatchUrl { get; } = $"apprenticeship/{apprenticeshipKey}/reverse-withdrawal";

    public ReverseWithdrawalRequest Data { get; set; } = new();
}

public class PauseRequest
{
    public DateTime PauseDate { get; set; }
}

public class PauseApiPatchRequest: IPatchApiRequest<PauseRequest>
{
    public string PatchUrl { get; }

    public PauseRequest Data { get; set; }

    public PauseApiPatchRequest(Guid apprenticeshipKey, PauseRequest data)
    {
        PatchUrl = $"apprenticeship/{apprenticeshipKey}/pause";
        Data = data;
    }
}

public class RemovePauseApiDeleteRequest(Guid apprenticeshipKey) : IDeleteApiRequest
{
    public string DeleteUrl { get; } = $"apprenticeship/{apprenticeshipKey}/pause";
}

public class MathsAndEnglishWithdrawApiPatchRequest(Guid apprenticeshipKey, MathsAndEnglishWithdrawRequest data)
    : IPatchApiRequest<MathsAndEnglishWithdrawRequest>
{
    public string PatchUrl { get; } =
        $"apprenticeship/{apprenticeshipKey}/mathsAndEnglish/withdraw";

    public MathsAndEnglishWithdrawRequest Data { get; set; } = data;
}

public class MathsAndEnglishWithdrawRequest
{
    public string Course { get; set; }
    public DateTime? WithdrawalDate { get; set; }
}

public class UpdateBreaksInLearningApiPatchRequest : IPatchApiRequest<UpdateBreaksInLearningRequest>
{
    public string PatchUrl { get; }
    public UpdateBreaksInLearningRequest Data { get; set; }

    public UpdateBreaksInLearningApiPatchRequest(Guid apprenticeshipKey, UpdateBreaksInLearningRequest data)
    {
        PatchUrl = $"apprenticeship/{apprenticeshipKey}/breaksInLearning";
        Data = data;
    }
}

public class UpdateBreaksInLearningRequest
{
    public Guid EpisodeKey { get; set; }
    public List<BreakInLearning> BreaksInLearning { get; set; }
}