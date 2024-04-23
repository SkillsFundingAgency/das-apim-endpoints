using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations
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
