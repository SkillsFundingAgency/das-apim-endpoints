using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

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
    public List<ProviderAttributeAnnualSummaryItem> ProviderAttribute { get; set; }
}

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
    public List<ProviderAttributeAnnualSummaryItem> ProviderAttribute { get; set; }
}

public class ProviderAttributeAnnualSummaryItem
{
    public string Name { get; set; }
    public int Strength { get; set; }
    public int Weakness { get; set; }
}
