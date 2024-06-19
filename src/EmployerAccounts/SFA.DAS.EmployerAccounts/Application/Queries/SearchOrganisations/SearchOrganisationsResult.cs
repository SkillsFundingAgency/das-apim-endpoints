using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerAccounts.Application.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsResult
    {
        public List<OrganisationResult> Organisations { get; set; }

        public static implicit operator SearchOrganisationsResult(EducationalOrganisationResponse organisationResponse)
        {
            if (organisationResponse == null)
            {
                return null;
            }

            return new SearchOrganisationsResult
            {
                Organisations = organisationResponse.EducationalOrganisations.Select(x => (OrganisationResult)x).ToList()
            };
        }
    }
}
