using SFA.DAS.SharedOuterApi.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Charities
{
    public class SearchCharitiesRequest : IGetApiRequest
    {
        public string SearchTerm { get; }
        public int MaximumResults { get; set; }

        public SearchCharitiesRequest(string searchTerm, int maximumResults)
        {
            SearchTerm = searchTerm;
            MaximumResults = maximumResults;
        }

        public string GetUrl => $"/api/Charities/search?searchTerm={HttpUtility.UrlEncode(SearchTerm)}&maximumResults={MaximumResults}";
    }
}
