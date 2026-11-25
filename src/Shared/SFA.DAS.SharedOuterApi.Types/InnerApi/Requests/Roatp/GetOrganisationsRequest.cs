using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;

public class GetOrganisationsRequest : IGetApiRequest
{
    public string GetUrl => ("organisations");
}