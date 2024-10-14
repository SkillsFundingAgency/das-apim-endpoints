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
        public string? Name { get; set; }
        public string? AddressLine1 { get; set; }
        public string? Town { get; set; }
        public string? County { get; set; }
        public string? PostCode { get; set; }
        public string? URN { get; set; }

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

