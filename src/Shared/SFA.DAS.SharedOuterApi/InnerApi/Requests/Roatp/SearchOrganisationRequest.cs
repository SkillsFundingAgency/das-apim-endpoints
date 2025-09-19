using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
public class SearchOrganisationRequest : IGetApiRequest
{
    public string SearchTerm { get; }

    public SearchOrganisationRequest(string searchTerm)
    {
        SearchTerm = searchTerm;
    }
    public string GetUrl => $"api/v1/Search?searchTerm={SearchTerm}";
}
