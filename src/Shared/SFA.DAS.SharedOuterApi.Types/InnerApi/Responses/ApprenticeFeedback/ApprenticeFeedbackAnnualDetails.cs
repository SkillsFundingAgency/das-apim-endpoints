using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Feedback;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ApprenticeFeedback;

public class ApprenticeFeedbackAnnualDetails
{
    public List<ApprenticeFeedbackStarsAnnualSummary> AnnualApprenticeFeedbackDetails { get; set; }
}

public class ApprenticeFeedbackStarsAnnualSummary
{
    public long Ukprn { get; set; }
    public int Stars { get; set; }
    public int ReviewCount { get; set; }
    public string TimePeriod { get; set; }
    public List<ApprenticeProviderAttributeAnnualSummaryItem> ProviderAttribute { get; set; }
}
