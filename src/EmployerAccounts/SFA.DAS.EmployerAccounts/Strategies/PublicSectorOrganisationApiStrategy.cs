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
    public class PublicSectorOrganisationApiStrategy : IOrganisationApiStrategy
    {
        private readonly IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> _psOrgApi;

        public PublicSectorOrganisationApiStrategy(IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration> psOrgApi)
        {
            _psOrgApi = psOrgApi;
        }

        public async Task<GetLatestDetailsResult> GetOrganisationDetails(string identifier, OrganisationType orgType)
        {
            var response = await _psOrgApi.GetWithResponseCode<PublicSectorOrganisation>(new GetLatestDetailsForPublicSectorOrganisationRequest(identifier));
            OrganisationApiResponseHelper.CheckApiResponseStatus(response.StatusCode, orgType, identifier, response.ErrorContent);

            return new GetLatestDetailsResult
            {
                OrganisationDetail = response.Body 
            };
        }
    }
}
