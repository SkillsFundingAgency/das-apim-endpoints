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

    [HttpGet("/api/rollover/rolloverworkflowcandidates")]
    [ProducesResponseType(typeof(GetRolloverWorkflowCandidatesQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRolloverWorkflowCandidates()
    {
        return await SendRequestAsync(new GetRolloverWorkflowCandidatesQuery());
    }

    [HttpPost("/api/rollover/rolloverworkflowruns")]
    [ProducesResponseType(typeof(BaseMediatrResponse<CreateRolloverWorkflowRunCommandResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRolloverWorkflowRun(CreateRolloverWorkflowRunCommand createRolloverWorkflowRunCommand)
    {
        return await SendRequestAsync(createRolloverWorkflowRunCommand);
    }

    [HttpGet("/api/rollover/{rolloverWorkflowRunId}/rollovercandidatesforexport")]
    [ProducesResponseType(typeof(GetRolloverCandidatesForExportQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRolloverCandidatesForExport(Guid rolloverWorkflowRunId)
    {
        return await SendRequestAsync(new GetRolloverCandidatesForExportQuery { RolloverWorkflowRunId = rolloverWorkflowRunId });
    }

    [HttpPost("/api/rollover/rolloverextensionvalidation")]
    [ProducesResponseType(typeof(BaseMediatrResponse<ValidateFundingExtensionCandidatesCommandResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ValidateFundingExtensionCandidates(ValidateFundingExtensionCandidatesCommand validateFundingExtensionCandidatesCommand)
    {
        return await SendRequestAsync(validateFundingExtensionCandidatesCommand);
    }
}