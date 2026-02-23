using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ToolsSupport.Application.Queries;
using System.Net;

namespace SFA.DAS.ToolsSupport.Api.Controllers;

[ApiController]
[Route("cohorts")]
public class CohortsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CohortsController> _logger;

    public CohortsController(IMediator mediator, ILogger<CohortsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        try
        {
            var response = await _mediator.Send(new GetCohortSupportApprenticeshipsQuery { CohortId = id });
            if (response == null)
                return NotFound();
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error attempting to query cohort and status by Id");
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}