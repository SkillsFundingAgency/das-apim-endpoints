using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Application.Queries;
using System.Net;

namespace SFA.DAS.ToolsSupport.Api.Controllers;

[ApiController]
[Route("apprenticeships")]
public class ApprenticeshipsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ApprenticeshipsController> _logger;

    public ApprenticeshipsController(IMediator mediator, ILogger<ApprenticeshipsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var response = await _mediator.Send(new GetApprenticeshipQuery() { Id = id });
            if(response == null)
                return NotFound();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error attempting to query by apprenticeship Id {id}");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("uln/{uln}")]
    public async Task<IActionResult> GetByUln(string uln)
    {
        try
        {
            var response = await _mediator.Send(new GetUlnSupportApprenticeshipsQuery() { Uln = uln });
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error attempting to query by ULN {uln}");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
