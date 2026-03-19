using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models.EmployerProfiles;
using SFA.DAS.Recruit.Application.EmployerProfile.Queries.GetEmployerProfileByLegalEntityId;
using SFA.DAS.Recruit.Application.EmployerProfile.Queries.GetEmployerProfilesByAccountId;
using System;
using System.Threading.Tasks;
using SFA.DAS.Recruit.Application.EmployerProfile.Commands.PatchEmployerProfile;
using SFA.DAS.Recruit.InnerApi.Models;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("employer")]
public class EmployerProfileController(IMediator mediator, ILogger<EmployerProfileController> logger) : ControllerBase
{
    [HttpGet]
    [Route("{accountId:long}/profiles")]
    public async Task<IResult> GetMany([FromRoute] long accountId)
    {
        try
        {
            var queryResult = await mediator.Send(new GetEmployerProfilesByAccountIdQuery(accountId));

            return Results.Ok(queryResult);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting profile by account id");
            return Results.Problem();
        }
    }

    [HttpGet]
    [Route("profiles/{accountLegalEntityId:long}")]
    public async Task<IResult> GetOne([FromRoute] long accountLegalEntityId)
    {
        try
        {
            var queryResult = await mediator.Send(new GetEmployerProfileByLegalEntityIdQuery(accountLegalEntityId));

            return Results.Ok(queryResult);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error getting profile by legal entity id");
            return Results.Problem();
        }
    }

    [HttpPost]
    [Route("profiles/{accountLegalEntityId:long}")]
    public async Task<IResult> PutOne([FromRoute] long accountLegalEntityId,
        [FromBody] PostEmployerProfileApiRequest request)
    {
        try
        {
            await mediator.Send(request.ToCommand(accountLegalEntityId));

            return Results.Created();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while posting profile by legal entity id");
            return Results.Problem();
        }
    }

    [HttpPut]
    [Route("profiles/{accountLegalEntityId:long}")]
    public async Task<IResult> PatchOne([FromRoute] long accountLegalEntityId,
        [FromBody] EmployerProfile request)
    {
        try
        {
            await mediator.Send(new PatchEmployerProfileCommand(accountLegalEntityId, request));

            return Results.NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while updating profile by legal entity id");
            return Results.Problem();
        }
    }
}
