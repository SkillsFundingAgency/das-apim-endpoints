using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations
{
    public class GetLatestDetailsForEducationalOrgRequest : IGetApiRequest
    {
        public string Identifier { get; }

        public GetLatestDetailsForEducationalOrgRequest(string identifier)
        {
            Identifier = identifier;
        }

        public string GetUrl => $"/api/EducationalOrganisations/LatestDetails?identifier={HttpUtility.UrlEncode(Identifier)}";
    }
}