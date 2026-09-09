using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Feedback;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerFeedback;

public class EmployerFeedbackAnnualDetails
{
    public List<EmployerFeedbackStarsAnnualSummary> AnnualEmployerFeedbackDetails { get; set; }
}

public class EmployerFeedbackStarsAnnualSummary
{
    public long Ukprn { get; set; }
    public int Stars { get; set; }
    public int ReviewCount { get; set; }
    public string TimePeriod { get; set; }
    public List<EmployerProviderAttributeAnnualSummaryItem> ProviderAttribute { get; set; }
}