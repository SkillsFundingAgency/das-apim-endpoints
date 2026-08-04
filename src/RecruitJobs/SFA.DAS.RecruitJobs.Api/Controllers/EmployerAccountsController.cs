using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitJobs.Api.Models.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[ApiController]
[Route("[controller]/{accountId:long}")]
public class EmployerAccountsController: ControllerBase
{
    [HttpGet]
    [Route("legalentities")]
    public async Task<IResult> GetAccountLegalEntities(
        [FromServices] IAccountsApiClient<AccountsConfiguration> apiClient,
        [FromRoute] long accountId)
    {
        var response = await apiClient.GetAll<GetAccountLegalEntityResponseItem>(new GetAccountLegalEntitiesRequest(accountId));

        return TypedResults.Ok(GetAccountLegalEntitiesResponse.From(response));
    }
}