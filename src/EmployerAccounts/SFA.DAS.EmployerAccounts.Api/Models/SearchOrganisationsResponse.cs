using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class SearchOrganisationsResponse
    {
        public IEnumerable<Organisation> Organisations { get; set; }

        public static implicit operator SearchOrganisationsResponse(SearchOrganisationsResult source)
        {
            return new SearchOrganisationsResponse
            {
                Organisations = source.Organisations.Select(x => (Organisation)x)

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

            public static implicit operator Organisation(SearchOrganisationsResult.Organisation source)
            {
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
