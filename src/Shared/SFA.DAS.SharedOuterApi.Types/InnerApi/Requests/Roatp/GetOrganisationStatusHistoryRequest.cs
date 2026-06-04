using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;

public class GetOrganisationStatusHistoryRequest(int ukprn) : IGetApiRequest
{
    public int Ukprn { get; set; } = ukprn;

    public string GetUrl => $"organisations/{Ukprn}/status-history";
}
