using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData
{
    public class GetSearchOrganisationsRequest : IGetApiRequest
    {
        public string SearchTerm { get; }
        public int MaximumResults { get; set; }

        public GetSearchOrganisationsRequest(string searchTerm, int maximumResults)
        {
            SearchTerm = searchTerm;
            MaximumResults = maximumResults;
        }

        public string GetUrl => $"api/organisations?searchTerm={HttpUtility.UrlEncode(SearchTerm)}&maximumResults={MaximumResults}";
    }
}
