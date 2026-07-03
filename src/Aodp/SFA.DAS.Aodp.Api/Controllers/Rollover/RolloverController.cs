using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.Rollover;
using SFA.DAS.Aodp.Application.Queries.Rollover;
using SFA.DAS.AODP.Application.Commands.Rollover;

namespace SFA.DAS.Aodp.Api.Controllers.Rollover;

[ApiController]
[Route("api/[controller]")]
public class RolloverController : BaseController
{
    private readonly ILogger<RolloverController> _logger;

    public RolloverController(IMediator mediator, ILogger<RolloverController> logger) : base(mediator, logger)
    {
        _logger = logger;
    }

    [HttpGet("/api/rollover/workflowcandidatescount")]
    [ProducesResponseType(typeof(GetRolloverWorkflowCandidatesCountQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRolloverWorkflowCandidatesCount(CancellationToken cancellationToken)
    {
        var query = new GetRolloverWorkflowCandidatesCountQuery();
        return await SendRequestAsync(query);
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

    [HttpGet("/api/rollover/{rolloverWorkflowRunId}/rollovercandidatesforexport")]
    [ProducesResponseType(typeof(GetRolloverCandidatesForExportQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRolloverCandidatesForExport(Guid rolloverWorkflowRunId)
    {
        return await SendRequestAsync(new GetRolloverCandidatesForExportQuery { RolloverWorkflowRunId = rolloverWorkflowRunId });
    }

    [HttpPost("/api/rollover/validaterolloverextension")]
    [ProducesResponseType(typeof(ValidateRolloverExtensionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ValidateRolloverExtension(ValidateRolloverExtensionCommand validateFundingExtensionCandidatesCommand)
    {
        return await SendRequestAsync(validateFundingExtensionCandidatesCommand);
    }

    [HttpPost("/api/rollover/submitrolloverextension")]
    [ProducesResponseType(typeof(SubmitRolloverExtensionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SubmitRolloverExtension(SubmitRolloverExtensionCommand submitRolloverExtensionCommand)
    {
        return await SendRequestAsync(submitRolloverExtensionCommand);
    }

    [HttpPost("/api/rollover/removepreviousworkflowcandidates")]
    [ProducesResponseType(typeof(RemovePreviousWorkflowCandidatesCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemovePreviousWorkflowCandidates(RemovePreviousWorkflowCandidatesCommand removePreviousWorkflowCandidatesCommand)
    {
        return await SendRequestAsync(removePreviousWorkflowCandidatesCommand);
    }
}