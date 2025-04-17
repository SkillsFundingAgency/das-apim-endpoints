using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

public sealed class GetApprenticeFeedbackSummaryAnnualResponse
{
    public IEnumerable<AnnualApprenticeFeedbackDetailsModel> AnnualApprenticeFeedbackDetails { get; set; }
}

public class AnnualApprenticeFeedbackDetailsModel
{
    public long Ukprn { get; set; }
    public int Stars { get; set; }
    public int ReviewCount { get; set; }
    public IEnumerable<AttributeResult> ProviderAttribute { get; set; }
    public string TimePeriod { get; set; }
}

public class AttributeResult
{
    public string Name { get; set; }
    public string Category { get; set; }
    public int Agree { get; set; }
    public int Disagree { get; set; }
}