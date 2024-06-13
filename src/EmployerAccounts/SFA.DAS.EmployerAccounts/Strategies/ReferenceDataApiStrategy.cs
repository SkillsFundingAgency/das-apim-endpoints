using System.Threading.Tasks;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;
using SFA.DAS.EmployerAccounts.Helpers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Strategies
{
    public class ReferenceDataApiStrategy : IOrganisationApiStrategy
    {
        private readonly IReferenceDataApiClient<ReferenceDataApiConfiguration> _refDataApi;

        public ReferenceDataApiStrategy(IReferenceDataApiClient<ReferenceDataApiConfiguration> refDataApi)
        {
            _refDataApi = refDataApi;
        }

        public async Task<GetLatestDetailsResult> GetOrganisationDetails(string identifier, OrganisationType orgType)
        {
            var response = await _refDataApi.GetWithResponseCode<Organisation>(new GetLatestDetailsRequest(identifier, orgType));
            OrganisationApiResponseHelper.CheckApiResponseStatus(response.StatusCode, orgType, identifier, response.ErrorContent);
            return new GetLatestDetailsResult
            {
                OrganisationDetail = response.Body
            };
        }
    }
}