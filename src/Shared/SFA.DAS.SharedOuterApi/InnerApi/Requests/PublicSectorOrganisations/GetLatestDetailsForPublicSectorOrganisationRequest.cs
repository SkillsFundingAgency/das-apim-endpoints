using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations
{
    public class GetLatestDetailsForPublicSectorOrganisationRequest : IGetApiRequest
    {
        public string Identifier { get; }

        public GetLatestDetailsForPublicSectorOrganisationRequest(string identifier)
        {
            Identifier = identifier;
        }

        public string GetUrl => $"/PublicSectorOrganisations/{HttpUtility.UrlEncode(Identifier)}";
    }
}