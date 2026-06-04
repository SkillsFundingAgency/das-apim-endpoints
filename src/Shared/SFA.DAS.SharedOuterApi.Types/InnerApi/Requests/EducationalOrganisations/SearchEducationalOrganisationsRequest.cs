using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EducationalOrganisations;

public class SearchEducationalOrganisationsRequest(string searchTerm, int maximumResults) : IGetApiRequest
{
    public string SearchTerm { get; } = searchTerm;
    public int MaximumResults { get; set; } = maximumResults;

    public string GetUrl => $"/api/EducationalOrganisations/search?searchTerm={HttpUtility.UrlEncode(SearchTerm)}&maximumResults={MaximumResults}";
}