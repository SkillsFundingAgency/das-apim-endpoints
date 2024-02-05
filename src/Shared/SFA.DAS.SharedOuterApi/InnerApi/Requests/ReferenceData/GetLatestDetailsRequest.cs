using System.Web;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData
{
    public class GetLatestDetailsRequest : IGetApiRequest
    {
        public string Identifier { get; }
        public OrganisationType OrganisationType { get; set; }

        public GetLatestDetailsRequest(string identifier, OrganisationType organisationType)
        {
            Identifier = identifier;
            OrganisationType = organisationType;
        }

        public string GetUrl => $"get?identifier={HttpUtility.UrlEncode(Identifier)}&organisationType={OrganisationType}";
    }
}