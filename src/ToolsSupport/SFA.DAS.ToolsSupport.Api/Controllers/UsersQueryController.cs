using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;
using SFA.DAS.ToolsSupport.Application.Queries;
using SFA.DAS.ToolsSupport.Application.Queries.GetUserOverview;
using SFA.DAS.ToolsSupport.Application.Queries.GetUserByUserRef;
using System.Net;

namespace SFA.DAS.ToolsSupport.Api.Controllers;

[ApiController]
[Route("users")]
public class UsersQueryController(IMediator mediator, ILogger<UsersQueryController> logger) : ControllerBase
{
    [HttpGet]
    [Route("query")]
    public async Task<IActionResult> Get([FromQuery] string email)
    {
        try
        {
            var response = await mediator.Send(new GetUsersByEmailQuery {Email = email});

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error attempting to query by email");
            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
    
    [HttpGet]
    [Route("user-overview")]
    public async Task<IActionResult> GetUserOverview([FromQuery] Guid userId)
    {
        try
        {
            var response = await mediator.Send(
                new GetUserOverviewQuery 
                { 
                    UserId = userId
                });

            return Ok((GetUserOverviewResponse)response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error attempting to get user summary");
            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
    
    [HttpGet]
    [Route("{userRef}")]
    public async Task<IActionResult> GetUserByUserRef([FromRoute] string userRef)
    {
        try
        {
            var response = await mediator.Send(
                new GetUserByUserRefQuery 
                { 
                    UserRef = userRef
                });

            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error attempting to get user by UserRef");
            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}

