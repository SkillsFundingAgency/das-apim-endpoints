using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.FormBuilder.Routes;
using SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes;

namespace SFA.DAS.Aodp.Api.Controllers.FormBuilder;

[ApiController]
[Route("api/[controller]")]
public class RoutesController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<RoutesController> _logger;

    public RoutesController(IMediator mediator, ILogger<RoutesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("/api/routes/forms/{formVersionId}/available-sections")]
    [ProducesResponseType(typeof(GetAvailableSectionsAndPagesForRoutingQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAvailableSectionsAndPagesForRouting(Guid formVersionId)
    {
        var query = new GetAvailableSectionsAndPagesForRoutingQuery()
        {
            FormVersionId = formVersionId
        };
        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return Ok(response.Value);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpGet("/api/routes/forms/{formVersionId}")]
    [ProducesResponseType(typeof(GetRoutingInformationForFormQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRoutesByFormVersionId(Guid formVersionId)
    {
        var query = new GetRoutingInformationForFormQuery()
        {
            FormVersionId = formVersionId
        };
        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpGet("/api/routes/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}/available-questions")]
    [ProducesResponseType(typeof(GetAvailableQuestionsForRoutingQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAvailableQuestionsForRouting(Guid pageId)
    {
        var query = new GetAvailableQuestionsForRoutingQuery()
        {
            PageId = pageId
        };
        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpGet("/api/routes/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}/questions/{questionId}")]
    [ProducesResponseType(typeof(GetRoutingInformationForQuestionQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetQuestionRoutingInformation(Guid questionId, Guid formVersionId)
    {
        var query = new GetRoutingInformationForQuestionQuery()
        {
            QuestionId = questionId,
            FormVersionId = formVersionId
        };

        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/routes/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}/questions/{questionId}")]
    [ProducesResponseType(typeof(GetRoutingInformationForQuestionQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConfigureRoutingForQuestion(ConfigureRoutingForQuestionCommand command)
    {
        var response = await _mediator.Send(command);
        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
