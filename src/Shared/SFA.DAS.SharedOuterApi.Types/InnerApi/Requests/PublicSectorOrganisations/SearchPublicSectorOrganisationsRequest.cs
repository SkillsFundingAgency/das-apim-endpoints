using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.PublicSectorOrganisations;

public class SearchPublicSectorOrganisationsRequest(string searchTerm) : IGetApiRequest
{
    public string SearchTerm { get; } = searchTerm;

    public string GetUrl => $"/PublicSectorOrganisations?searchTerm={HttpUtility.UrlEncode(SearchTerm)}";
}