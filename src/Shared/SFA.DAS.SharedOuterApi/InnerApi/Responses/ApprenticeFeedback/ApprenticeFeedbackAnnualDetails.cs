using SFA.DAS.SharedOuterApi.InnerApi.Responses.Feedback;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeFeedback;
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
