using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

public class UpdateEnglishAndMathsApiPutRequest(Guid learningKey, UpdateEnglishAndMathsRequest data)
    : IPutApiRequest<UpdateEnglishAndMathsRequest>
{
    public string PutUrl { get; } = $"learning/{learningKey}/english-and-maths";
    public UpdateEnglishAndMathsRequest Data { get; set; } = data;
}

public class UpdateEnglishAndMathsRequest
{
    public List<EnglishAndMathsItem> EnglishAndMaths { get; set; } = [];
}

[DebuggerDisplay("Start={StartDate.ToString(\"yyyy-MM-dd\")}, End={EndDate.ToString(\"yyyy-MM-dd\")}, LearnAimRef={LearnAimRef}")]
public class EnglishAndMathsItem
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Course { get; set; } = null!;
    public string LearnAimRef { get; set; }
    public decimal Amount { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public int? PriorLearningAdjustmentPercentage { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public DateTime? PauseDate { get; set; }
    public List<PeriodInLearningItem> PeriodsInLearning { get; set; } = [];
}
