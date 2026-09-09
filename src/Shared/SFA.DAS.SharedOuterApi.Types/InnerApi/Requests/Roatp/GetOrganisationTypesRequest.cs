using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;

public class GetOrganisationTypesRequest : IGetApiRequest
{
    public string GetUrl => "organisation-types";
}