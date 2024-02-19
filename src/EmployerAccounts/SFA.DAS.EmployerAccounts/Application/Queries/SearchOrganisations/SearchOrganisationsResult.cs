using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerAccounts.Application.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsResult
    {
        public List<Organisation> Organisations { get; set; }

        public static implicit operator SearchOrganisationsResult(EducationalOrganisationResponse organisationResponse)
        {
            return new SearchOrganisationsResult
            {
                Organisations = organisationResponse.EducationalOrganisations.Select(x => (Organisation)x).ToList()
            };
        }
    }
}
