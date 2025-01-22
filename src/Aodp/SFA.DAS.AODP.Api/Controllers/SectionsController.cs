using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AODP.Application.Commands.FormBuilder.Sections;
using SFA.DAS.AODP.Application.Queries.FormBuilder.Sections;

namespace SFA.DAS.AODP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SectionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SectionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/api/sections/form/{formId}")]
    [ProducesResponseType(typeof(GetAllSectionsQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync([FromRoute] Guid formVersionId)
    {
        var query = new GetAllSectionsQuery(formVersionId);
        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return Ok(response);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }

    [HttpGet("/api/sections/{sectionId}/form/{formId}")]
    [ProducesResponseType(typeof(GetSectionByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid sectionId, [FromRoute] Guid formVersionId)
    {
        var query = new GetSectionByIdQuery(sectionId, formVersionId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            if (response.Data is null)
                return NotFound();
            return Ok(response);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }

    [HttpPost("/api/sections")]
    [ProducesResponseType(typeof(CreateSectionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSectionCommand.Section section)
    {
        var command = new CreateSectionCommand(section);

        var response = await _mediator.Send(command);
        if (response.Success)
        {
            if (response.Data is null)
                return NotFound();
            return Ok(response);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }

    [HttpPut("/api/sections/{formId}")]
    [ProducesResponseType(typeof(UpdateSectionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid formVersionId, [FromBody] UpdateSectionCommand.Section section)
    {
        var command = new UpdateSectionCommand(formVersionId, section);

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            if (response.Data is null)
                return NotFound();
            return Ok(response);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }

    [HttpDelete("/api/sections/{sectionId}")]
    [ProducesResponseType(typeof(DeleteSectionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync([FromRoute] Guid sectionId)
    {
        var command = new DeleteSectionCommand(sectionId);

        var response = await _mediator.Send(command);
        if (response.Success)
        {
            return Ok(response);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }
}