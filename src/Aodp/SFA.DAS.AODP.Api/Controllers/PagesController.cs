using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AODP.Application.Commands.FormBuilder.Pages;
using SFA.DAS.AODP.Application.FormBuilder.Pages.Queries;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Pages;

namespace SFA.DAS.AODP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PagesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet("/api/pages/section/{sectionId}")]
    [ProducesResponseType(typeof(List<GetAllPagesApiResponse.Page>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync(Guid sectionId)
    {
        var query = new GetAllPagesQuery(sectionId);

        var response = await _mediator.Send(query);
        if (response.Success)
        {
            var result = _mapper.Map<GetAllPagesApiResponse>(response);
            return Ok(result.Data);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }

    [HttpGet("/api/pages/{pageId}/section/{sectionId}")]
    [ProducesResponseType(typeof(List<GetPageByIdApiResponse.Page>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(Guid pageId, Guid sectionId)
    {
        var query = new GetPageByIdQuery(pageId, sectionId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            var result = _mapper.Map<GetPageByIdApiResponse>(response);
            if (result.Data is null)
                return NotFound();
            return Ok(result.Data);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }

    [HttpPost("/api/pages")]
    [ProducesResponseType(typeof(CreatePageApiResponse.Page), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePageCommand.Page page)
    {
        var command = new CreatePageCommand(page);

        var response = await _mediator.Send(command);
        if (response.Success)
        {
            var result = _mapper.Map<CreatePageApiResponse>(response);
            if (result.Data is null)
                return NotFound();
            return Ok(result.Data);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }

    [HttpPut("/api/pages/{pageId}")]
    [ProducesResponseType(typeof(UpdatePageApiResponse.Page), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid pageId, [FromBody] UpdatePageCommand.Page page)
    {
        var command = new UpdatePageCommand(pageId, page);

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            var result = _mapper.Map<UpdatePageApiResponse>(response);
            if (result.Data is null)
                return NotFound();
            return Ok(result.Data);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }

    [HttpDelete("/api/pages/{pageId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync([FromRoute] Guid pageId)
    {
        var command = new DeletePageCommand(pageId);

        var response = await _mediator.Send(command);
        if (response.Success)
        {
            var result = new DeletePageApiResponse();
            result.Data = response.Data;
            return Ok(result.Data);
        }

        var errorObjectResult = new ObjectResult(response.ErrorMessage);
        errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

        return errorObjectResult;
    }
}