using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.ExternalApi.Requests
{
    public class SearchCompanyInformationRequest : IGetApiRequest
    {
        public string SearchTerm { get; }
        public int MaximumResults { get; set; }

        public SearchCompanyInformationRequest(string searchTerm, int maximumResults)
        {
            SearchTerm = string.IsNullOrEmpty(searchTerm) ? string.Empty : searchTerm.ToUpper();
            MaximumResults = maximumResults;
        }

        public string GetUrl => $"search/companies?q={HttpUtility.UrlEncode(SearchTerm)}&items_per_page={MaximumResults}";
    }
}
