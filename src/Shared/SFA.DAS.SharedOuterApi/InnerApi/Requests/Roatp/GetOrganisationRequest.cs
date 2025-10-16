using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
public class GetOrganisationRequest : IGetApiRequest
{
    public int Ukprn { get; set; }

    public GetOrganisationRequest(int ukprn)
    {
        Ukprn = ukprn;
    }

    public string GetUrl => $"organisations/{Ukprn}";
}
