using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.Application.Message;

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


    //[HttpGet("/api/applications/{applicationId}/messages")]
    //[ProducesResponseType(typeof(GetApplicationMessagesByIdQueryResponse), StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //public async Task<IActionResult> GetApplicationMessagesByIdAsync(Guid applicationId)
    //{
    //    var query = new GetApplicationMessagesByIdQuery(applicationId);
    //    return await SendRequestAsync(query);
    //}

    [HttpPost("/api/applicationMessages/{applicationId}/messages")]
    [ProducesResponseType(typeof(CreateApplicationMessageCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateApplicationMessageAsync([FromBody] CreateApplicationMessageCommand command, [FromRoute] Guid applicationId)
    {
        return await SendRequestAsync(command);
    }
}