using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public sealed class GetEmployerFeedbackSummaryAnnualRequest : IGetApiRequest
{
    public string GetUrl => $"api/employerfeedbackresult/{Ukprn}/annual";
    private long Ukprn { get; }
    public GetEmployerFeedbackSummaryAnnualRequest(long ukprn)
    {
        Ukprn = ukprn;
    }
}