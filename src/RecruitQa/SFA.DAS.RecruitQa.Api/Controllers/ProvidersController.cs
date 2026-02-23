using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Models.Responses;
using SFA.DAS.RecruitQa.Application.Provider.GetProvider;
using System.Net;

namespace SFA.DAS.RecruitQa.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class ProvidersController(IMediator mediator, ILogger<ProvidersController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{ukprn:int}")]
    public async Task<IResult> GetProvider([FromRoute] int ukprn)
    {
        try
        {
            var response = await mediator.Send(new GetProviderQuery(ukprn));
            return TypedResults.Ok((GetProviderResponse)response.Provider);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting provider information");
            return Results.Problem(statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }
}