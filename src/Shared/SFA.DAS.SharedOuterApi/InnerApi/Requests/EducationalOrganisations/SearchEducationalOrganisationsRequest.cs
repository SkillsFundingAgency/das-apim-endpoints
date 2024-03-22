using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations
{
    public class SearchEducationalOrganisationsRequest : IGetApiRequest
    {
        public string SearchTerm { get; }
        public int MaximumResults { get; set; }

        public SearchEducationalOrganisationsRequest(string searchTerm, int maximumResults)
        {
            SearchTerm = searchTerm;
            MaximumResults = maximumResults;
        }

        public string GetUrl => $"/api/EducationalOrganisations/search?searchTerm={HttpUtility.UrlEncode(SearchTerm)}&maximumResults={MaximumResults}";
    }
}
