using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Application.Queries.Application.Application;

namespace SFA.DAS.Aodp.Api.Controllers.Application;

[ApiController]
[Route("api/[controller]")]
public class ApplicationMessagesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<ApplicationMessagesController> _logger;

    public ApplicationMessagesController(IMediator mediator, ILogger<ApplicationMessagesController> logger) : base(mediator, logger)
    {
        _mediator = mediator;
        _logger = logger;
    }


    [HttpGet("/api/applications/{applicationId}/messages")]
    [ProducesResponseType(typeof(GetApplicationMessagesByApplicationIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplicationMessagesByIdAsync([FromRoute] Guid applicationId, [FromQuery] string userType)
    {
        var query = new GetApplicationMessagesByApplicationIdQuery(applicationId, userType);
        return await SendRequestAsync(query);
    }

    [HttpGet("/api/applications/messages/{messageId}")]
    [ProducesResponseType(typeof(GetApplicationMessageByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMessageByIdAsync([FromRoute] Guid messageId)
    {
        var query = new GetApplicationMessageByIdQuery(messageId);
        return await SendRequestAsync(query);
    }

    [HttpPost("/api/applications/{applicationId}/messages")]
    [ProducesResponseType(typeof(CreateApplicationMessageCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateApplicationMessageAsync([FromBody] CreateApplicationMessageCommand command, [FromRoute] Guid applicationId)
    {
        return await SendRequestAsync(command);
    }

    [HttpPut("/api/applications/{applicationId}/messages/read")]
    [ProducesResponseType(typeof(EmptyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MarkAllMessagesAsRead([FromBody] MarkAllMessagesAsReadCommand command, [FromRoute] Guid applicationId)
    {
        command.ApplicationId = applicationId;

        return await SendRequestAsync(command);

    }
}