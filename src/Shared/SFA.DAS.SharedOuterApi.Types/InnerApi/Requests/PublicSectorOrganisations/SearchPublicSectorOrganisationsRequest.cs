using System.Web;

using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.PublicSectorOrganisations
{
    public class SearchPublicSectorOrganisationsRequest : IGetApiRequest
    {
        public string SearchTerm { get; }

        public SearchPublicSectorOrganisationsRequest(string searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public string GetUrl => $"/PublicSectorOrganisations?searchTerm={HttpUtility.UrlEncode(SearchTerm)}";
    }
}