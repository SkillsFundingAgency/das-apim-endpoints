using SFA.DAS.EmployerAccounts.Application.Queries.GetIdentifiableOrganisationTypes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class GetIdentifiableOrganisationTypesResponse
    {
        public OrganisationType[] OrganisationTypes { get; set; }

        public static explicit operator GetIdentifiableOrganisationTypesResponse(GetIdentifiableOrganisationTypesResult source)
        {
            return new GetIdentifiableOrganisationTypesResponse
            {
                OrganisationTypes = source.OrganisationTypes
            };
        }
    }
}
