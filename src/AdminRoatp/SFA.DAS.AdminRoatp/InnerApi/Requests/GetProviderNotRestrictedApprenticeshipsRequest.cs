using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class GetProviderNotRestrictedApprenticeshipsRequest : IGetApiRequest
{
    public int Ukprn { get; set; }
    public string GetUrl => $"providers/{Ukprn}/not-restricted-apprenticeships";

    public GetProviderNotRestrictedApprenticeshipsRequest(int ukprn)
    {
        Ukprn = ukprn;
    }
}
