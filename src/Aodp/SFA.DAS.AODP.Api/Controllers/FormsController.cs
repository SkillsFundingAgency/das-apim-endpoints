﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;
using SFA.DAS.AODP.Application.Queries.FormBuilder.Forms;

namespace SFA.DAS.AODP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<FormsController> _logger;

    public FormsController(IMediator mediator, ILogger<FormsController> logger)
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
            return Ok(response);
        }

        _logger.LogError(message: $"Error thrown getting all forms versions: {response.ErrorMessage}");
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
            return Ok(response);
        }

        _logger.LogError(message: $"Error thrown getting a form version with the Id `{formVersionId}`: {response.ErrorMessage}");
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
            return Ok(response);
        }

        _logger.LogError(message: $"Error thrown creating a form and form version: {response.ErrorMessage}");
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formVersionId}")]
    [ProducesResponseType(typeof(UpdateFormVersionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(Guid formVersionId, [FromBody] UpdateFormVersionCommand command)
    {
        command.FormVersionId = formVersionId;

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            return Ok(response);
        }

        _logger.LogError(message: $"Error thrown updating a form version with the Id `{formVersionId}`: {response.ErrorMessage}");
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formVersionId}/publish")]
    [ProducesResponseType(typeof(UpdateFormVersionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PublishAsync(Guid formVersionId)
    {
        var command = new PublishFormVersionCommand(formVersionId);

        var response = await _mediator.Send(command);

        if (response.Success)
            return Ok(response);

        _logger.LogError(message: $"Error thrown publishing a form version with the Id `{formVersionId}`: : {response.ErrorMessage}");
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formVersionId}/unpublish")]
    [ProducesResponseType(typeof(UpdateFormVersionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UnpublishAsync(Guid formVersionId)
    {
        var command = new UnpublishFormVersionCommand(formVersionId);

        var response = await _mediator.Send(command);

        if (response.Success)
            return Ok(response);

        _logger.LogError(message: $"Error thrown unpublishing a form version with the Id `{formVersionId}`: : {response.ErrorMessage}");
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpDelete("/api/forms/{formVersionId}")]
    [ProducesResponseType(typeof(DeleteFormVersionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(Guid formVersionId)
    {
        var command = new DeleteFormVersionCommand(formVersionId);

        var response = await _mediator.Send(command);
        if (response.Success)
        {
            return Ok(response);
        }

        _logger.LogError(message: $"Error thrown deleting a form version with the Id `{formVersionId}`: {response.ErrorMessage}");
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
