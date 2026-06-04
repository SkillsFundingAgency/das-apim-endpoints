using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ReferenceData;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ReferenceData;

public class GetLatestDetailsRequest(string identifier, OrganisationType organisationType) : IGetApiRequest
{
    public string Identifier { get; } = identifier;
    public OrganisationType OrganisationType { get; set; } = organisationType;

    public string GetUrl => $"api/organisations/get?identifier={HttpUtility.UrlEncode(Identifier)}&organisationType={OrganisationType}";
}