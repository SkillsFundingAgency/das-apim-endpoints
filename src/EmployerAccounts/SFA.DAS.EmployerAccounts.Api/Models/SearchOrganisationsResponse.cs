using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class SearchOrganisationsResponse
    {
        public IEnumerable<OrganisationResponse> Organisations { get; set; }

        public static implicit operator SearchOrganisationsResponse(SearchOrganisationsResult source)
        {
            return new SearchOrganisationsResponse
            {
                Organisations = source.Organisations.Select(x => (OrganisationResponse)x)
            };
        }
    }
}
