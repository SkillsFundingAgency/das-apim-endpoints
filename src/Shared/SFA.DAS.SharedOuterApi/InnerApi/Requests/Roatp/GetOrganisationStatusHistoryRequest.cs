using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;

public class GetOrganisationStatusHistoryRequest : IGetApiRequest
{
    public int Ukprn { get; set; }
    public GetOrganisationStatusHistoryRequest(int ukprn)
    {
        Ukprn = ukprn;
    }
    public string GetUrl => $"organisations/{Ukprn}/status-history";
}
