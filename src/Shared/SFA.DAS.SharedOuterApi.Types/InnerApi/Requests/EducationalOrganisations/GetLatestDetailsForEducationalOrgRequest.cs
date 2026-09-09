using SFA.DAS.Apim.Shared.Interfaces;
using System.Web;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EducationalOrganisations;

public class GetLatestDetailsForEducationalOrgRequest(string identifier) : IGetApiRequest
{
    public string Identifier { get; } = identifier;

    public string GetUrl => $"/api/EducationalOrganisations/LatestDetails?identifier={HttpUtility.UrlEncode(Identifier)}";
}