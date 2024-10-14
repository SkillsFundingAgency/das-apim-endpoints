using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisationsByLepCode
{
    public class GetEducationalOrganisationsByLepCodeResult
    {
        public int TotalCount { get; set; }
        public ICollection<EducationalOrganisationData>? EducationalOrganisations { get; set; }
    }
}