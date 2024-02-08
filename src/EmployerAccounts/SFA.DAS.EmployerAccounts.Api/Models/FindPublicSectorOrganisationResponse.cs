using System.Collections.Generic;
using SFA.DAS.EmployerAccounts.Application.Queries.FindPublicSectorOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class FindPublicSectorOrganisationResponse
    {
        public List<PublicSectorOrganisation> Data { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }

        public class PublicSectorOrganisation
        {
            public string Name { get; set; }

            public DataSource Source { get; set; }

            public string Sector { get; set; }

            public string AddressLine1 { get; set; }

            public string AddressLine2 { get; set; }

            public string AddressLine3 { get; set; }

            public string AddressLine4 { get; set; }

            public string AddressLine5 { get; set; }

            public string PostCode { get; set; }

            public string OrganisationCode { get; set; }


            public static implicit operator PublicSectorOrganisation(FindPublicSectorOrganisationResult source)
            {
                if (source == null)
                {
                    return null;
                }

                return new PublicSectorOrganisation
                {
                    Name = source.Name,
                    Source = source.Source,
                    Sector = source.Sector,
                    AddressLine1 = source.AddressLine1,
                    AddressLine2 = source.AddressLine2,
                    AddressLine3 = source.AddressLine3,
                    AddressLine4 = source.AddressLine4,
                    AddressLine5 = source.AddressLine5,
                    PostCode = source.PostCode,
                    OrganisationCode = source.OrganisationCode
                };
            }
        }
    }
}
