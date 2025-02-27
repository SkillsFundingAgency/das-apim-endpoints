﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.Aodp.Api.Controllers.Application;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(IMediator mediator, ILogger<ApplicationsController> logger) : base(mediator, logger)
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
        return await SendRequestAsync(query);
    }


    [HttpGet("/api/applications/forms/{formVersionId}/sections/{sectionId}/pages/{pageOrder}")]
    [ProducesResponseType(typeof(GetApplicationPageByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(int pageOrder, Guid sectionId, Guid formVersionId)
    {
        var query = new GetApplicationPageByIdQuery(pageOrder, sectionId, formVersionId);
        return await SendRequestAsync(query);
    }

    [HttpGet("/api/applications/forms/{formVersionId}")]
    [ProducesResponseType(typeof(GetApplicationFormByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFormVersionByIdAsync(Guid formVersionId)
    {
        var query = new GetApplicationFormByIdQuery(formVersionId);
        return await SendRequestAsync(query);
    }

    [HttpGet("/api/applications/organisations/{organisationId}")]
    [ProducesResponseType(typeof(GetApplicationsByOrganisationIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationsByOrganisationId(Guid organisationId)
    {
        var query = new GetApplicationsByOrganisationIdQuery(organisationId);
        return await SendRequestAsync(query);
    }



    [HttpGet("/api/applications/forms/{formVersionId}/sections/{sectionId}")]
    [ProducesResponseType(typeof(GetApplicationSectionByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSectionByIdAsync(Guid sectionId, Guid formVersionId)
    {
        var query = new GetApplicationSectionByIdQuery(sectionId, formVersionId);
        return await SendRequestAsync(query);
    }



    [HttpGet("/api/applications/{applicationId}/forms/{formVersionId}/sections/{sectionId}")]
    [ProducesResponseType(typeof(GetApplicationSectionStatusByApplicationIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationPagesForSectionByIdAsync(Guid applicationId, Guid sectionId, Guid formVersionId)
    {
        var query = new GetApplicationSectionStatusByApplicationIdQuery(sectionId, formVersionId, applicationId);
        return await SendRequestAsync(query);
    }


    [HttpGet("/api/applications/{applicationId}/forms/{formVersionId}")]
    [ProducesResponseType(typeof(GetApplicationFormStatusByApplicationIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationSectionsForFormByIdAsync(Guid applicationId, Guid formVersionId)
    {
        var query = new GetApplicationFormStatusByApplicationIdQuery(formVersionId, applicationId);
        return await SendRequestAsync(query);
    }

    [HttpGet("/api/applications/{applicationId}/form-preview")]
    [ProducesResponseType(typeof(GetApplicationFormPreviewByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationFormPreviewByIdAsync(Guid applicationId)
    {
        var query = new GetApplicationFormPreviewByIdQuery(applicationId);
        return await SendRequestAsync(query);
    }



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
        return await SendRequestAsync(query);
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

        return await SendRequestAsync(command);
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

}
