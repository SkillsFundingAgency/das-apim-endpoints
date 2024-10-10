using SFA.DAS.SharedOuterApi.Interfaces;
using System.Web;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class GetEducationalOrganisationsByLepCodeRequest : IGetApiRequest
    {
        public string LepCode { get; set; } 
        public string? SearchTerm { get; set; }
        public GetEducationalOrganisationsByLepCodeRequest(string lepCode, string? searchTerm)
        {
            LepCode = lepCode;
            SearchTerm = searchTerm;
        }

        public string GetUrl => $"api/educational-organisations-data/?LepCode={HttpUtility.UrlEncode(LepCode)}&SearchTerm={HttpUtility.UrlEncode(SearchTerm)}";
    }
}