using SFA.DAS.SharedOuterApi.Interfaces;
using System.Web;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class GetEducationalOrganisationsByLepCodeRequest : IGetApiRequest
    {
        public string LepCode { get; set; } 
        public string? SearchTerm { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public GetEducationalOrganisationsByLepCodeRequest(string lepCode, string? searchTerm, int page, int pageSize)
        {
            LepCode = lepCode;
            SearchTerm = searchTerm;
            Page = page;
            PageSize = pageSize;
        }

        public string GetUrl => $"api/educational-organisations-data/?LepCode={HttpUtility.UrlEncode(LepCode)}&SearchTerm={HttpUtility.UrlEncode(SearchTerm)}&Page={Page}&PageSize={PageSize}";
    }
}