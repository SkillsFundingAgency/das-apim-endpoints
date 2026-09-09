using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Charities;

public class SearchCharitiesRequest(string searchTerm, int maximumResults) : IGetApiRequest
{
    public string SearchTerm { get; } = searchTerm;
    public int MaximumResults { get; set; } = maximumResults;

    public string GetUrl => $"/api/Charities/search?searchTerm={HttpUtility.UrlEncode(SearchTerm)}&maximumResults={MaximumResults}";
}