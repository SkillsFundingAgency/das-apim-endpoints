using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Forms;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Forms;

namespace SFA.DAS.Aodp.Api.Controllers.FormBuilder;

[ApiController]
[Route("api/[controller]")]
public class FormsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<FormsController> _logger;

    public FormsController(IMediator mediator, ILogger<FormsController> logger) : base(mediator, logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("/api/forms")]
    [ProducesResponseType(typeof(GetAllFormVersionsQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        var query = new GetAllFormVersionsQuery();
        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return Ok(response.Value);
        }

        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpGet("/api/forms/{formVersionId}")]
    [ProducesResponseType(typeof(GetFormVersionByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(Guid formVersionId)
    {
        var query = new GetFormVersionByIdQuery(formVersionId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }
        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPost("/api/forms")]
    [ProducesResponseType(typeof(CreateFormVersionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateFormVersionCommand command)
    {
        var response = await _mediator.Send(command);
        if (response.Success)
        {
            return Ok(response.Value);
        }
        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formVersionId}")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(Guid formVersionId, [FromBody] UpdateFormVersionCommand command)
    {
        command.FormVersionId = formVersionId;

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            return Ok(response.Value);
        }

        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formVersionId}/publish")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PublishAsync(Guid formVersionId)
    {
        var command = new PublishFormVersionCommand(formVersionId);

        var response = await _mediator.Send(command);

        if (response.Success)
            return Ok(response.Value);

        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formVersionId}/unpublish")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UnpublishAsync(Guid formVersionId)
    {
        var command = new UnpublishFormVersionCommand(formVersionId);

        var response = await _mediator.Send(command);

        if (response.Success)
            return Ok(response.Value);

        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formId}/MoveUp")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MoveUpAsync(Guid formId)
    {
        var command = new MoveFormUpCommand(formId);

        var response = await _mediator.Send(command);

        if (response.Success)
            return Ok(response.Value);

        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formId}/MoveDown")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MoveDownAsync(Guid formId)
    {
        var command = new MoveFormDownCommand(formId);

        var response = await _mediator.Send(command);

        if (response.Success)
            return Ok(response.Value);

        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }


    [HttpPut("/api/forms/{formId}/new-version")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateDraftAsync([FromRoute] Guid formId)
    {
        var command = new CreateDraftFormVersionCommand(formId);

        var response = await _mediator.Send(command);

        if (response.Success)
            return Ok(response.Value);

        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpDelete("/api/forms/{formVersionId}")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(Guid formVersionId)
    {
        var command = new DeleteFormVersionCommand(formVersionId);

        var response = await _mediator.Send(command);
        if (response.Success)
        {
            return Ok(response.Value);
        }
        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
