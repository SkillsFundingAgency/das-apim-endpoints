using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations
{
    public class IdentifiableOrganisationTypesRequest : IGetApiRequest
    {
        public string GetUrl => "api/EducationalOrganisations/IdentifiableOrganisationTypes";
    }
}
