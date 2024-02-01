using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsResult
    {
        public IEnumerable<Organisation> Organisations { get; set; }

        public static implicit operator SearchOrganisationsResult(SearchOrganisationsResponse organisations)
        {
            return new SearchOrganisationsResult
            {
                Organisations = organisations.Organisations.Select(x => (Organisation)x)
            };
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

            public static implicit operator Organisation(SearchOrganisationsResponse.Organisation source)
            {
                if (source == null)
                {
                    return null;
                }

                return new Organisation
                {
                    Name = source.Name,
                    Type = source.Type,
                    SubType = source.SubType,
                    Code = source.Code,
                    RegistrationDate = source.RegistrationDate,
                    Address = source.Address,
                    Sector = source.Sector,
                    OrganisationStatus = source.OrganisationStatus
                };
            }
        }
    }
}
