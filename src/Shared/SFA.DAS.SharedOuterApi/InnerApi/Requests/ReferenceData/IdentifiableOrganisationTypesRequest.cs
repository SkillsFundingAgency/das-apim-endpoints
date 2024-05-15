using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData
{
    public class IdentifiableOrganisationTypesRequest : IGetApiRequest
    {
        public string GetUrl => "api/organisations/IdentifiableOrganisationTypes";
    }
}
