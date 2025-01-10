using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;
using SFA.DAS.EmployerAccounts.Helpers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Strategies;

public class CharitiesApiStrategy(ICharitiesApiClient<CharitiesApiConfiguration> charitiesApi) : IOrganisationApiStrategy
{
    public async Task<GetLatestDetailsResult> GetOrganisationDetails(string identifier, OrganisationType orgType)
    {
        if (!int.TryParse(identifier, out int registrationNumber))
        {
            throw new BadHttpRequestException("Charity identifier must be a valid integer.");
        }
        
        var response = await charitiesApi.GetWithResponseCode<GetCharityResponse>(new GetCharityRequest(registrationNumber));
        OrganisationApiResponseHelper.CheckApiResponseStatus(response.StatusCode, orgType, identifier, response.ErrorContent);
        return new GetLatestDetailsResult
        {
            OrganisationDetail = response.Body
        };
    }
}