using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Application.Queries;
using System.Net;

namespace SFA.DAS.ToolsSupport.Api.Controllers;

[ApiController]
[Route("accounts")]
public class EmployerAccountsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmployerAccountsController> _logger;

    public EmployerAccountsController(IMediator mediator, ILogger<EmployerAccountsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] long? accountId, [FromQuery] string? payeSchemeRef)
    {
        try
        {
            var response = await _mediator.Send(new GetEmployerAccountsQuery {AccountId = accountId, PayeSchemeRef = payeSchemeRef});

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error attempting to query Employer Account using accountId {0} or PayeSchemeRef {1}", accountId, payeSchemeRef);
            return StatusCode((int) HttpStatusCode.InternalServerError);
        }
    }
}