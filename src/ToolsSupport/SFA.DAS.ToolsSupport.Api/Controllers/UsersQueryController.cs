using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Api.Models.EmployerAccount;
using SFA.DAS.ToolsSupport.Application.Queries;
using SFA.DAS.ToolsSupport.Application.Queries.GetUserOverview;
using System.Net;

namespace SFA.DAS.ToolsSupport.Api.Controllers;

[ApiController]
[Route("users/query")]
public class UsersQueryController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersQueryController> _logger;

    public UsersQueryController(IMediator mediator, ILogger<UsersQueryController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string email)
    {
        try
        {
            var response = await _mediator.Send(new GetUsersByEmailQuery {Email = email});

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error attempting to query by email");
            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
    
    [HttpGet]
    [Route("user-overview")]
    public async Task<IActionResult> GetUserOverview([FromQuery] Guid userId)
    {
        try
        {
            var response = await _mediator.Send(
                new GetUserOverviewQuery 
                { 
                    UserId = userId
                });

            return Ok((GetUserOverviewResponse)response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error attempting to get user summary");
            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}

