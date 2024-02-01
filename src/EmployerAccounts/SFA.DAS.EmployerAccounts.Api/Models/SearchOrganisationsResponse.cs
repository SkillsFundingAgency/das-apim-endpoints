using System.Collections.Generic;
using SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class SearchOrganisationsResponse
    {
        public IEnumerable<Organisation> Organisations { get; set; }

        public static implicit operator SearchOrganisationsResponse(SearchOrganisationsResult source)
        {
            return new SearchOrganisationsResponse
            {
                Organisations = source.Organisations
             
            };
        }
    }
}
