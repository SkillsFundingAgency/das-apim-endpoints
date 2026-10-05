using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class GetProviderRestrictedApprenticeshipsRequest : IGetApiRequest
{
    public int Ukprn { get; set; }
    public string GetUrl => $"providers/{Ukprn}/restricted-apprenticeships";

    public GetProviderRestrictedApprenticeshipsRequest(int ukprn)
    {
        Ukprn = ukprn;
    }
}
