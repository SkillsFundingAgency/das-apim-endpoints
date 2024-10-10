using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisationsByLepCode
{
    public class GetEducationalOrganisationsByLepCodeResult
    {
        public ICollection<GetEducationalOrganisationsByLepCodeResponse>? EducationalOrganisations { get; set; }
    }
}