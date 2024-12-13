using SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisationsByLepCode;
using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class GetEducationalOrganisationsResponse
    {

        public int TotalCount { get; set; }
        public ICollection<EducationalOrganisation>? EducationalOrganisations { get; set; }

        public static implicit operator GetEducationalOrganisationsResponse(GetEducationalOrganisationsByLepCodeResult r)
        {
            return new GetEducationalOrganisationsResponse
            {
                TotalCount = r.TotalCount,
                EducationalOrganisations = r.EducationalOrganisations
                    .Select(org => (EducationalOrganisation)org)
                    .ToList()
            };
        }
    }
    public class EducationalOrganisation
    {
        public string Name { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string Town { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;
        public string URN { get; set; } = string.Empty;

        public static implicit operator EducationalOrganisation(EducationalOrganisationData educationalOrganisationData)
        {
            return new EducationalOrganisation
            {
                Name = educationalOrganisationData.Name,
                AddressLine1 = educationalOrganisationData.AddressLine1,
                Town = educationalOrganisationData.Town,
                County = educationalOrganisationData.County,
                PostCode = educationalOrganisationData.PostCode,
                URN = educationalOrganisationData.URN
            };
        }
    }
}

