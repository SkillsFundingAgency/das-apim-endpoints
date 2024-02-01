using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsResult
    {
        public IEnumerable<Organisation> Organisations { get; set; }
    }

    public class Organisation
    {
        public string Name { get; set; }
        public OrganisationType Type { get; set; }
        public OrganisationSubType SubType { get; set; }
        public string Code { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public Address Address { get; set; }
        public string Sector { get; set; }
        public OrganisationStatus OrganisationStatus { get; set; }
    }
}
