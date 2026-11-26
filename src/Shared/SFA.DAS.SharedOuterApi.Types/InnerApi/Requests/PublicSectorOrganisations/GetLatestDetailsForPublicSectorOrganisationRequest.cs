using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.PublicSectorOrganisations;

public class GetLatestDetailsForPublicSectorOrganisationRequest(string identifier) : IGetApiRequest
{
    public string Identifier { get; } = identifier;

    public string GetUrl => $"/PublicSectorOrganisations/{HttpUtility.UrlEncode(Identifier)}";
}