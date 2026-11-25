using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ReferenceData;

public class GetSearchOrganisationsRequest(string searchTerm, int maximumResults) : IGetApiRequest
{
    public string SearchTerm { get; } = searchTerm;
    public int MaximumResults { get; set; } = maximumResults;

    public string GetUrl => $"api/organisations/?searchTerm={HttpUtility.UrlEncode(SearchTerm)}&maximumResults={MaximumResults}";
}