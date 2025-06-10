using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
public class GetProviderSummaryRequest : IGetApiRequest
{
    public int Ukprn { get; init; }
    public GetProviderSummaryRequest(int ukprn)
    {
        Ukprn = ukprn;
    }

    public string GetUrl => $"api/providers/{Ukprn}/summary";
}
