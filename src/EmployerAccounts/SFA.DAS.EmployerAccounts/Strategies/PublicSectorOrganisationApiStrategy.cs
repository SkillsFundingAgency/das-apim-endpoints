using System.Threading.Tasks;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;
using SFA.DAS.EmployerAccounts.Helpers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PublicSectorOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Strategies
{
    public class PublicSectorOrganisationApiStrategy(
        IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> psOrgApi)
        : IOrganisationApiStrategy
    {
        public async Task<GetLatestDetailsResult> GetOrganisationDetails(string identifier, OrganisationType orgType)
        {
            var response = await psOrgApi.GetWithResponseCode<PublicSectorOrganisation>(new GetLatestDetailsForPublicSectorOrganisationRequest(identifier));
            OrganisationApiResponseHelper.CheckApiResponseStatus(response.StatusCode, orgType, identifier, response.ErrorContent);

            return new GetLatestDetailsResult
            {
                OrganisationDetail = response.Body 
            };
        }
    }
}
