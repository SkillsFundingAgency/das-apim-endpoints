using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Pages;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;
using SFA.DAS.AODP.Api;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.Aodp.Api.Controllers.FormBuilder;
[ApiController]
[Route("api/[controller]")]
public class PagesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<FormsController> _logger;

    public PagesController(IMediator mediator, ILogger<FormsController> logger) : base(mediator, logger)
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

        return await SendRequestAsync(query);
    }

    [HttpGet("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}")]
    [ProducesResponseType(typeof(GetPageByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(Guid formVersionId, Guid pageId, Guid sectionId)
    {
        var query = new GetPageByIdQuery(pageId, sectionId, formVersionId);

        return await SendRequestAsync(query);
    }

    [HttpGet("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}/preview")]
    [ProducesResponseType(typeof(GetPageByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPagePreviewByIdAsync(Guid formVersionId, Guid pageId, Guid sectionId)
    {
        var query = new GetPagePreviewByIdQuery(pageId, sectionId, formVersionId);

        return await SendRequestAsync(query);
    }

    [HttpPost("/api/forms/{formVersionId}/sections/{sectionId}/pages")]
    [ProducesResponseType(typeof(CreatePageCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId, [FromBody] CreatePageCommand command)
    {
        command.FormVersionId = formVersionId;
        command.SectionId = sectionId;

        return await SendRequestAsync(command);
    }

    [HttpPut("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId, [FromRoute] Guid pageId, [FromBody] UpdatePageCommand command)
    {
        command.FormVersionId = formVersionId;
        command.SectionId = sectionId;
        command.Id = pageId;

        return await SendRequestAsync(command);

    }

    [HttpPut("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}/MoveUp")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MoveUpAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId, [FromRoute] Guid pageId)
    {
        var query = new MovePageUpCommand()
        {
            SectionId = sectionId,
            FormVersionId = formVersionId,
            PageId = pageId
        };

        return await SendRequestAsync(query);
    }


    [HttpPut("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}/MoveDown")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MoveDownAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId, [FromRoute] Guid pageId)
    {
        var query = new MovePageDownCommand()
        {
            SectionId = sectionId,
            FormVersionId = formVersionId,
            PageId = pageId
        };

        return await SendRequestAsync(query);
    }

    [HttpDelete("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync([FromRoute] Guid pageId, [FromRoute] Guid formVersionId, [FromRoute] Guid sectionId)
    {
        var command = new DeletePageCommand()
        {
            PageId = pageId,
            FormVersionId = formVersionId,
            SectionId = sectionId
        };

        return await SendRequestAsync(command);
    }
}