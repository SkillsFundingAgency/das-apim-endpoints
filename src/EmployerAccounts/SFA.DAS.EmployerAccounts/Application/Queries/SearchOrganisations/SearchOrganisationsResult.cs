using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation.EducationalOrganisationResponse;

namespace SFA.DAS.EmployerAccounts.Application.Queries.SearchOrganisations
{
    public class SearchOrganisationsResult
    {
        public IEnumerable<Organisation> Organisations { get; set; }

        public static implicit operator SearchOrganisationsResult(EducationalOrganisationResponse organisationResponse)
        {
            return new SearchOrganisationsResult
            {
                Organisations = organisationResponse.EducationalOrganisations.Select(x => (Organisation)x)
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

            public static implicit operator Organisation(EducationalOrganisation source)
            {
                if (source == null)
                {
                    return null;
                }

                return new Organisation
                {
                    Name = source.Name,
                    Type = OrganisationType.EducationOrganisation,
                    SubType = OrganisationSubType.None,
                    Code = source.URN,
                    RegistrationDate = null,
                    Address = new Address
                    {
                        Line1 = source.AddressLine1,
                        Line2 = source.AddressLine2,
                        Line3 = source.AddressLine3,
                        Line4 = source.Town,
                        Line5 = source.County,
                        Postcode = source.PostCode
                    },
                    Sector = source.EducationalType
                };
            }
        }
    }
}
