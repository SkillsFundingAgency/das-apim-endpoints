using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

public sealed class GetEmployerFeedbackSummaryAnnualResponse
{
    public IEnumerable<AnnualEmployerFeedbackDetailsModel> AnnualEmployerFeedbackDetails { get; set; }
}

public sealed class AnnualEmployerFeedbackDetailsModel
{
    public long Ukprn { get; set; }

    public int Stars { get; set; }

    public int ReviewCount { get; set; }

    public string TimePeriod { get; set; }

    public IEnumerable<ProviderAttributeModel> ProviderAttribute { get; set; }
}

public sealed class ProviderAttributeModel
{
    public string Name { get; set; }

    public int Strength { get; set; }

    public int Weakness { get; set; }
}
