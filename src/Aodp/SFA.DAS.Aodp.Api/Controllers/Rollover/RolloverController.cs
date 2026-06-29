using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.Rollover;
using SFA.DAS.Aodp.Application.Queries.Rollover;

namespace SFA.DAS.Aodp.Api.Controllers.Rollover;

[ApiController]
[Route("api/[controller]")]
public class RolloverController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<RolloverController> _logger;

    public RolloverController(IMediator mediator, ILogger<RolloverController> logger) : base(mediator, logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("/api/rollover/workflowcandidatescount")]
    [ProducesResponseType(typeof(GetRolloverWorkflowCandidatesCountQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRolloverWorkflowCandidatesCount(CancellationToken cancellationToken)
    {
        var query = new GetRolloverWorkflowCandidatesCountQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("/api/rollover/rollovercandidates")]
    [ProducesResponseType(typeof(GetRolloverCandidatesQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRolloverCandidates()
    {
        return await SendRequestAsync(new GetRolloverCandidatesQuery());
    }

    [HttpPost("/api/rollover/querybuilder/awardingorganisations")]
    [ProducesResponseType(typeof(GetAwardingOrganisationsForRolloverQueryBuilderQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAwardingOrganisationsForRolloverQueryBuilder(
        [FromBody] RolloverQueryBuilderRequest filters)
    {
        return await SendRequestAsync(new GetAwardingOrganisationsForRolloverQueryBuilderQuery(filters));
    }

    [HttpPost("/api/rollover/querybuilder/qualificationversions")]
    [ProducesResponseType(typeof(GetQualificationVersionsForRolloverQueryBuilderQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetQualificationVersionsForRolloverQueryBuilder(
        [FromBody] RolloverQueryBuilderRequest filters)
    {
        return await SendRequestAsync(new GetQualificationVersionsForRolloverQueryBuilderQuery(filters));
    }

    [HttpPost("/api/rollover/rolloverworkflowruns")]
    [ProducesResponseType(typeof(BaseMediatrResponse<CreateRolloverWorkflowRunCommandResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRolloverWorkflowRun(CreateRolloverWorkflowRunCommand createRolloverWorkflowRunCommand)
    {
        return await SendRequestAsync(createRolloverWorkflowRunCommand);
    }
}
