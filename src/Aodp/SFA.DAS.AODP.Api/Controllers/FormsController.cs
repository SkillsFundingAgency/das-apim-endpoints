using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;
using SFA.DAS.AODP.Application.Queries.FormBuilder.Forms;

namespace SFA.DAS.AODP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormsController : Controller
{
    private readonly IMediator _mediator;

    public FormsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/api/forms")]
    [ProducesResponseType(typeof(GetAllFormVersionsQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        var query = new GetAllFormVersionsQuery();
        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return Ok(response);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }

    [HttpGet("/api/forms/{formVersionId}")]
    [ProducesResponseType(typeof(GetFormVersionByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(Guid formVersionId)
    {
        var request = new GetFormVersionByIdQuery(formVersionId);

        var response = await _mediator.Send(request);

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

    [HttpPost("/api/forms")]
    [ProducesResponseType(typeof(CreateFormVersionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateFormVersionCommand.FormVersion formVersion)
    {
        var command = new CreateFormVersionCommand(formVersion);

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

    [HttpPut("/api/forms/{formVersionId}")]
    [ProducesResponseType(typeof(UpdateFormVersionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(Guid formVersionId, [FromBody] UpdateFormVersionCommand.FormVersion formVersion)
    {
        var command = new UpdateFormVersionCommand(formVersionId, formVersion);

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

    [HttpPut("/api/forms/{formVersionId}/publish")]
    [ProducesResponseType(typeof(PublishFormVersionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PublishAsync(Guid formVersionId)
    {
        var command = new PublishFormVersionCommand(formVersionId);

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            return Ok(response);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }

    [HttpPut("/api/forms/{formVersionId}/unpublish")]
    [ProducesResponseType(typeof(UnpublishFormVersionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UnpublishAsync(Guid formVersionId)
    {
        var command = new UnpublishFormVersionCommand(formVersionId);

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            return Ok(response);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }

    [HttpDelete("/api/forms/{formVersionId}")]
    [ProducesResponseType(typeof(DeleteFormVersionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(Guid formVersionId)
    {
        var command = new DeleteFormVersionCommand(formVersionId);

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
