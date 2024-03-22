using SFA.DAS.EmployerAccounts.Application.Queries.GetIdentifiableOrganisationTypes;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class GetIdentifiableOrganisationTypesResponse
    {
        public string[] OrganisationTypes { get; set; }

        public static explicit operator GetIdentifiableOrganisationTypesResponse(GetIdentifiableOrganisationTypesResult source)
        {
            return new GetIdentifiableOrganisationTypesResponse
            {
                OrganisationTypes = source.OrganisationTypes
            };
        }
    }
}
