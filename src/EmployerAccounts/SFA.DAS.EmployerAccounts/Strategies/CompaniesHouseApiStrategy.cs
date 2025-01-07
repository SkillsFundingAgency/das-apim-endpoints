using System.Threading.Tasks;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;
using SFA.DAS.EmployerAccounts.Configuration;
using SFA.DAS.EmployerAccounts.ExternalApi;
using SFA.DAS.EmployerAccounts.ExternalApi.Requests;
using SFA.DAS.EmployerAccounts.ExternalApi.Responses;
using SFA.DAS.EmployerAccounts.Helpers;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Strategies
{
    public class CompaniesHouseApiStrategy(ICompaniesHouseApiClient<CompaniesHouseApiConfiguration> companiesHouseApi)
        : IOrganisationApiStrategy
    {
        public async Task<GetLatestDetailsResult> GetOrganisationDetails(string identifier, OrganisationType orgType)
        {
            var response = await companiesHouseApi.GetWithResponseCode<GetCompanyInfoResponse>(new GetCompanyInformationRequest(identifier));
            OrganisationApiResponseHelper.CheckApiResponseStatus(response.StatusCode, orgType, identifier, response.ErrorContent);
            return new GetLatestDetailsResult
            {
                OrganisationDetail = response.Body
            };
        }
    }
}