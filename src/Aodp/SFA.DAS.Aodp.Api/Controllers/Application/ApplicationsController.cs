using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Aodp.Api.Controllers.Application;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(IMediator mediator, ILogger<ApplicationsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }


    [HttpGet("/api/applications/forms")]
    [ProducesResponseType(typeof(GetApplicationFormsQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllAsync()
    {
        var query = new GetApplicationFormsQuery();
        var response = await _mediator.Send(query);
        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }


    [HttpGet("/api/applications/forms/{formVersionId}/sections/{sectionId}/pages/{pageOrder}")]
    [ProducesResponseType(typeof(GetApplicationPageByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(int pageOrder, Guid sectionId, Guid formVersionId)
    {
        var query = new GetApplicationPageByIdQuery(pageOrder, sectionId, formVersionId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }


    [HttpGet("/api/applications/forms/{formVersionId}")]
    [ProducesResponseType(typeof(GetApplicationFormByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFormVersionByIdAsync(Guid formVersionId)
    {
        var query = new GetApplicationFormByIdQuery(formVersionId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpGet("/api/applications/organisations/{organisationId}")]
    [ProducesResponseType(typeof(GetApplicationsByOrganisationIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationsByOrganisationId(Guid organisationId)
    {
        var query = new GetApplicationsByOrganisationIdQuery(organisationId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }



    [HttpGet("/api/applications/forms/{formVersionId}/sections/{sectionId}")]
    [ProducesResponseType(typeof(GetApplicationSectionByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSectionByIdAsync(Guid sectionId, Guid formVersionId)
    {
        var query = new GetApplicationSectionByIdQuery(sectionId, formVersionId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }



    [HttpGet("/api/applications/{applicationId}/forms/{formVersionId}/sections/{sectionId}")]
    [ProducesResponseType(typeof(GetApplicationSectionStatusByApplicationIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationPagesForSectionByIdAsync(Guid applicationId, Guid sectionId, Guid formVersionId)
    {
        var query = new GetApplicationSectionStatusByApplicationIdQuery(sectionId, formVersionId, applicationId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }


    [HttpGet("/api/applications/{applicationId}/forms/{formVersionId}")]
    [ProducesResponseType(typeof(GetApplicationFormStatusByApplicationIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationSectionsForFormByIdAsync(Guid applicationId, Guid formVersionId)
    {
        var query = new GetApplicationFormStatusByApplicationIdQuery(formVersionId, applicationId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }



    [HttpGet("/api/applications/{applicationId}/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}/answers")]
    [ProducesResponseType(typeof(GetApplicationPageAnswersByPageIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationPageAnswersByIdAsync(Guid applicationId, Guid pageId, Guid sectionId, Guid formVersionId)
    {
        var query = new GetApplicationPageAnswersByPageIdQuery(applicationId, pageId, sectionId, formVersionId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/applications/{applicationId}/forms/{formVersionId}/sections/{sectionId}/pages/{pageId}")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAnswersAsync([FromRoute] Guid formVersionId, [FromRoute] Guid sectionId, [FromRoute] Guid pageId, [FromRoute] Guid applicationId, [FromBody] UpdatePageAnswersCommand command)
    {
        command.FormVersionId = formVersionId;
        command.SectionId = sectionId;
        command.ApplicationId = applicationId;
        command.PageId = pageId;

        var response = await _mediator.Send(command);

        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPost("/api/applications")]
    [ProducesResponseType(typeof(CreateApplicationCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateApplicationCommand command)
    {
        var response = await _mediator.Send(command);
        if (response.Success)
        {
            return Ok(response.Value);
        }

        return StatusCode(StatusCodes.Status500InternalServerError);
    }


    [HttpGet("/api/applications/{applicationId}/metadata")]
    [ProducesResponseType(typeof(GetApplicationMetadataByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationMetadataByIdAsync(Guid applicationId)
    {
        var query = new GetApplicationMetadataByIdQuery(applicationId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }
        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpGet("/api/applications/{applicationId}")]
    [ProducesResponseType(typeof(GetApplicationByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationByIdAsync(Guid applicationId)
    {
        var query = new GetApplicationByIdQuery(applicationId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }
        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }


    [HttpDelete("/api/applications/{applicationId}")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteApplicationByIdAsync(Guid applicationId)
    {
        var query = new DeleteApplicationCommand(applicationId);

        var response = await _mediator.Send(query);

        if (response.Success)
        {
            return Ok(response.Value);
        }
        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }

    [HttpPut("/api/applications/{applicationId}")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditAsync([FromBody] EditApplicationCommand command, [FromRoute] Guid applicationId)
    {
        command.ApplicationId = applicationId;

        var response = await _mediator.Send(command);
        if (response.Success)
        {
            return Ok(response.Value);
        }

        _logger.LogError(response.ErrorMessage);
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}
