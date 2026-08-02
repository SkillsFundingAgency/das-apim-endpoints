using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ValidateRolloverExtensionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ValidateRolloverExtension(
        IFormFile payload,
        CancellationToken cancellationToken)
    {
        var command = await ReadCommand<ValidateRolloverExtensionCommand>(payload, cancellationToken);
        return command is null
            ? BadRequest("The JSON payload is missing or invalid.")
            : await SendRequestAsync(command);
    }

    [HttpPost("/api/rollover/submitrolloverextension")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(SubmitRolloverExtensionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SubmitRolloverExtension(
        IFormFile payload,
        CancellationToken cancellationToken)
    {
        var command = await ReadCommand<SubmitRolloverExtensionCommand>(payload, cancellationToken);
        return command is null
            ? BadRequest("The JSON payload is missing or invalid.")
            : await SendRequestAsync(command);
    }

    [HttpPost("/api/rollover/removepreviousworkflowcandidates")]
    [ProducesResponseType(typeof(RemovePreviousWorkflowCandidatesCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemovePreviousWorkflowCandidates(RemovePreviousWorkflowCandidatesCommand removePreviousWorkflowCandidatesCommand)
    {
        return await SendRequestAsync(removePreviousWorkflowCandidatesCommand);
    }

    [HttpPost("/api/rollover/querybuilder/qualificationversions")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(GetQualificationVersionsForRolloverQueryBuilderQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetQualificationVersionsForRolloverQueryBuilder([FromForm] RolloverQueryBuilderRequest filters)
    {
        return await SendRequestAsync(new GetQualificationVersionsForRolloverQueryBuilderQuery(filters));
    }

    [HttpGet("/api/rollover/querybuilder/levels")]
    [ProducesResponseType(typeof(GetLevelsForRolloverQueryBuilderQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLevelsForRolloverQueryBuilder()
    {
        return await SendRequestAsync(new GetLevelsForRolloverQueryBuilderQuery());
    }

    [HttpPost("/api/rollover/querybuilder/types")]
    [ProducesResponseType(typeof(GetTypesForRolloverQueryBuilderQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTypesForRolloverQueryBuilder([FromBody] RolloverQueryBuilderTypesRequest filters)
    {
        return await SendRequestAsync(new GetTypesForRolloverQueryBuilderQuery(filters));
    }

    [HttpPost("/api/rollover/querybuilder/sectorsubjectarea")]
    [ProducesResponseType(typeof(GetSectorSubjectAreaForRolloverQueryBuilderQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSectorSubjectAreaForRolloverQueryBuilder([FromBody] RolloverQueryBuilderSectorSubjectAreaRequest filters)
    {
        return await SendRequestAsync(new GetSectorSubjectAreaForRolloverQueryBuilderQuery(filters));
    }

    [HttpPost("/api/rollover/querybuilder/awardingorganisations")]
    [ProducesResponseType(typeof(GetAwardingOrganisationsForRolloverQueryBuilderQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAwardingOrganisationsForRolloverQueryBuilder(
        [FromBody] RolloverQueryBuilderAwardingOrganisationsRequest filters)
    {
        return await SendRequestAsync(new GetAwardingOrganisationsForRolloverQueryBuilderQuery(filters));
    }
    private static async Task<TCommand?> ReadCommand<TCommand>(
        IFormFile payload,
        CancellationToken cancellationToken)
    {
        if (payload is null ||
            payload.Length == 0 ||
            !payload.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
        {
            return default;
        }

        try
        {
            await using var stream = payload.OpenReadStream();
            return await JsonSerializer.DeserializeAsync<TCommand>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }, cancellationToken);
        }
        catch (JsonException)
        {
            return default;
        }
    }

    [HttpGet("/api/rollover/startsummary")]
    [ProducesResponseType(typeof(GetRolloverStartSummaryQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRolloverStartSummary()
    {
        return await SendRequestAsync(new GetRolloverStartSummaryQuery());
    }
}
