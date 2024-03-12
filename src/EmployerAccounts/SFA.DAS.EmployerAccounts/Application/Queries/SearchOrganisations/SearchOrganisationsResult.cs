using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerAccounts.Application.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsResult
    {
        public IEnumerable<OrganisationResult> Organisations { get; set; }

        public static implicit operator SearchOrganisationsResult(GetSearchOrganisationsResponse organisations)
        {
            if (organisations == null)
            {
                return null;
            }

            return new SearchOrganisationsResult
            {
                Organisations = organisations.Select(x => (OrganisationResult)x)
            };
        }
    }
}
