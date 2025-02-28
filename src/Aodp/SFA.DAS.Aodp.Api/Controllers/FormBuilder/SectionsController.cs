using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Sections;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Sections;

namespace SFA.DAS.Aodp.Api.Controllers.FormBuilder;

[Route("api/[controller]")]
[ApiController]
public class SectionsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<SectionsController> _logger;

    public SectionsController(IMediator mediator, ILogger<SectionsController> logger) : base(mediator, logger)
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

        return await SendRequestAsync(query);
    }

    [HttpGet("/api/forms/{formVersionId}/sections/{sectionId}")]
    [ProducesResponseType(typeof(GetSectionByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid sectionId, [FromRoute] Guid formVersionId)
    {
        var query = new GetSectionByIdQuery(sectionId, formVersionId);

        return await SendRequestAsync(query);
    }

    [HttpPost("/api/forms/{formVersionId}/sections")]
    [ProducesResponseType(typeof(CreateSectionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(Guid formVersionId, [FromBody] CreateSectionCommand command)
    {
        command.FormVersionId = formVersionId;

        return await SendRequestAsync(command);
    }

    [HttpPut("/api/forms/{formVersionId}/sections/{sectionId}")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId, [FromBody] UpdateSectionCommand command)
    {
        command.FormVersionId = formVersionId;
        command.Id = sectionId;

        return await SendRequestAsync(command);
    }

    [HttpPut("/api/forms/{formVersionId}/sections/{sectionId}/MoveUp")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MoveUpAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId)
    {
        var query = new MoveSectionUpCommand()
        {
            SectionId = sectionId,
            FormVersionId = formVersionId
        };

        return await SendRequestAsync(query);
    }

    [HttpPut("/api/forms/{formVersionId}/sections/{sectionId}/MoveDown")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MoveDownAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId)
    {
        var query = new MoveSectionDownCommand()
        {
            SectionId = sectionId,
            FormVersionId = formVersionId,
        };

        return await SendRequestAsync(query);
    }

    [HttpDelete("/api/forms/{formVersionId}/sections/{sectionId}")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync([FromRoute] Guid sectionId, [FromRoute] Guid formVersionId)
    {
        var command = new DeleteSectionCommand()
        {
            SectionId = sectionId,
            FormVersionId = formVersionId
        };

        return await SendRequestAsync(command);
    }
}
