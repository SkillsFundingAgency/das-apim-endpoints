using System;
using System.Collections.Generic;
using System.Linq;
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
                Organisations = source.Organisations.Select(x => (Organisation)x)
            };
        }

        public class Organisation
        {
            public string Name { get; set; }
            public SharedOuterApi.InnerApi.Responses.ReferenceData.OrganisationType Type { get; set; }
            public SharedOuterApi.InnerApi.Responses.ReferenceData.OrganisationSubType SubType { get; set; }
            public string Code { get; set; }
            public DateTime? RegistrationDate { get; set; }
            public SharedOuterApi.InnerApi.Responses.ReferenceData.Address Address { get; set; }
            public string Sector { get; set; }
            public SharedOuterApi.InnerApi.Responses.ReferenceData.OrganisationStatus OrganisationStatus { get; set; }

            public static implicit operator Organisation(Application.Models.Organisation source)
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
