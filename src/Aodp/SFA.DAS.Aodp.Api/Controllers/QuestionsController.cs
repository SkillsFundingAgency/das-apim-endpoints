using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Questions;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Questions;

namespace SFA.DAS.Aodp.Api.Controllers;

public class QuestionsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<QuestionsController> _logger;

    public QuestionsController(IMediator mediator, ILogger<QuestionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}/questions")]
    [ProducesResponseType(typeof(CreateQuestionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId, [FromRoute] Guid pageId, [FromBody] CreateQuestionCommand command)
    {
        command.FormVersionId = formVersionId;
        command.SectionId = sectionId;
        command.PageId = pageId;

        var response = await _mediator.Send(command);
        if (response.Success && response.Value.Id != default)
        {
            return Ok(response.Value);
        }
        _logger.LogError(message: $"Error thrown creating question for page Id `{pageId}` and section Id `{sectionId}`: {response.ErrorMessage}");

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}/questions/{questionId}")]
    [ProducesResponseType(typeof(UpdateQuestionCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId, [FromRoute] Guid pageId, [FromRoute] Guid questionId, [FromBody] UpdateQuestionCommand command)
    {
        command.FormVersionId = formVersionId;
        command.SectionId = sectionId;
        command.Id = questionId;
        command.PageId = pageId;

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            return Ok(response.Value);
        }
        _logger.LogError(message: $"Error thrown editing question for Id `{questionId}` and page Id `{pageId}`: {response.ErrorMessage}");

        return StatusCode(StatusCodes.Status500InternalServerError);
    }


    [HttpGet("/api/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}/questions/{questionId}")]
    [ProducesResponseType(typeof(GetQuestionByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(Guid formVersionId, Guid pageId, Guid sectionId, Guid questionId)
    {
        var query = new GetQuestionByIdQuery()
        {
            QuestionId = questionId,
            FormVersionId = formVersionId,
            SectionId = sectionId,
            PageId = pageId
        };

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }
        _logger.LogError(message: $"Error thrown getting question for Id `{questionId}` and page Id `{pageId}`: {response.ErrorMessage}");
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
