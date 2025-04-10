using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public sealed class GetApprenticeFeedbackSummaryAnnualRequest : IGetApiRequest
{
    public string GetUrl => $"api/apprenticefeedbackresult/{Ukprn}/annual";
    private long Ukprn { get; }
    public GetApprenticeFeedbackSummaryAnnualRequest(long ukprn)
    {
        Ukprn = ukprn;
    }
}
