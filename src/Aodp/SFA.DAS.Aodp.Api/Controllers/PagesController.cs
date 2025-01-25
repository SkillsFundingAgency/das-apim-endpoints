using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;
using SFA.DAS.AODP.Application.Queries.FormBuilder.Pages;

namespace SFA.DAS.AODP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagesController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<FormsController> _logger;

    public PagesController(IMediator mediator, ILogger<FormsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("/api/forms/{formVersionId}/sections/{sectionId}/pages")]
    [ProducesResponseType(typeof(GetAllPagesQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync(Guid sectionId, Guid formVersionId)
    {
        var query = new GetAllPagesQuery()
        {
            SectionId = sectionId,
            FormVersionId = formVersionId
        };

        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return Ok(response);
        }

        _logger.LogError(message: $"Error thrown getting all pages for section Id `{sectionId}`.");
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpGet("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}")]
    [ProducesResponseType(typeof(GetPageByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(Guid formVersionId, Guid pageId, Guid sectionId)
    {
        var query = new GetPageByIdQuery()
        {
            FormVersionId = formVersionId,
            PageId = pageId,
            SectionId = sectionId
        };

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response);
        }

        _logger.LogError(message: $"Error thrown getting page for section Id `{sectionId}` and page Id `{pageId}`: {response.ErrorMessage}");
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPost("/api/forms/{formVersionId}/sections/{sectionId}/pages")]
    [ProducesResponseType(typeof(CreatePageCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId, [FromBody] CreatePageCommand command)
    {
        command.FormVersionId = formVersionId;
        command.SectionId = sectionId;

        var response = await _mediator.Send(command);
        if (response.Success && response.Id != default)
        {
            return Ok(response);
        }
        _logger.LogError(message: $"Request to add page with section Id `{command.SectionId}` but no section with this Id can be found: {response.ErrorMessage}");
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}")]
    [ProducesResponseType(typeof(UpdatePageCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId, [FromRoute] Guid pageId, [FromBody] UpdatePageCommand command)
    {
        command.FormVersionId = formVersionId;
        command.SectionId = sectionId;
        command.Id = pageId;

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            return Ok(response);
        }

        _logger.LogError($"Request to edit page with page Id `{pageId}` but no page with this Id can be found: {response.ErrorMessage}");

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpDelete("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}")]
    [ProducesResponseType(typeof(DeletePageCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(Guid formVersionId, Guid pageId, Guid sectionId)
    {
        var command = new DeletePageCommand()
        {
            FormVersionId = formVersionId,
            SectionId = sectionId,
            PageId = pageId
        };

        var response = await _mediator.Send(command);
        if (response.Success)
        {
            return Ok(response);
        }

        _logger.LogError($"Request to delete page with page Id `{pageId}` but no page with this Id can be found: {response.ErrorMessage}");


        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}