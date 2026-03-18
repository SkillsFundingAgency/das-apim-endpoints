using System.Diagnostics;

using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.LearnerData;

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
    public DateTime? PauseDate { get; set; }
    public DateTime? WithdrawalDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public int? PriorLearningAdjustmentPercentage { get; set; }
    public List<PeriodInLearningItem> PeriodsInLearning { get; set; } = [];
}
