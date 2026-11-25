using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EducationalOrganisations;

public class IdentifiableOrganisationTypesRequest : IGetApiRequest
{
    public string GetUrl => "api/EducationalOrganisations/IdentifiableOrganisationTypes";
}