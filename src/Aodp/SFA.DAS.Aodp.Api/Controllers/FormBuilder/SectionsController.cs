using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Sections;

namespace SFA.DAS.Aodp.Api.Controllers.FormBuilder;



[Route("api/[controller]")]
[ApiController]
public class SectionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SectionsController> _logger;

    public SectionsController(IMediator mediator, ILogger<SectionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("/api/forms/{formVersionId}/sections")]
    [ProducesResponseType(typeof(GetAllSectionsQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync([FromRoute] Guid formVersionId)
    {
        var query = new GetAllSectionsQuery(formVersionId);

        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return Ok(response.Value);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpGet("/api/forms/{formVersionId}/sections/{sectionId}")]
    [ProducesResponseType(typeof(GetSectionByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid sectionId, [FromRoute] Guid formVersionId)
    {
        var query = new GetSectionByIdQuery(sectionId, formVersionId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPost("/api/forms/{formVersionId}/sections")]
    [ProducesResponseType(typeof(CreateSectionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(Guid formVersionId, [FromBody] CreateSectionCommand command)
    {
        command.FormVersionId = formVersionId;

        var response = await _mediator.Send(command);
        if (response.Success)
        {
            return Ok(response.Value);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formVersionId}/sections/{sectionId}")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId, [FromBody] UpdateSectionCommand command)
    {
        command.FormVersionId = formVersionId;
        command.Id = sectionId;
        var response = await _mediator.Send(command);

        if (response.Success)
        {
            return Ok(response.Value);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formVersionId}/sections/{sectionId}/MoveUp")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MoveUpAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId)
    {
        var query = new MoveSectionUpCommand()
        {
            SectionId = sectionId,
            FormVersionId = formVersionId
        };

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formVersionId}/sections/{sectionId}/MoveDown")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MoveDownAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId)
    {
        var query = new MoveSectionDownCommand()
        {
            SectionId = sectionId,
            FormVersionId = formVersionId,
        };

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpDelete("/api/forms/{formVersionId}/sections/{sectionId}")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync([FromRoute] Guid sectionId, [FromRoute] Guid formVersionId)
    {
        var command = new DeleteSectionCommand()
        {
            SectionId = sectionId,
            FormVersionId = formVersionId
        };

        var response = await _mediator.Send(command);
        if (response.Success)
        {
            return Ok(response.Value);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
