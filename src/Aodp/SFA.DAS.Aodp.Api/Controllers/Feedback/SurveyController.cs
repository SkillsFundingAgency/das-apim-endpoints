using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aodp.Application.Commands.Feedback;

namespace SFA.DAS.Aodp.Api.Controllers.Feedback;

[ApiController]
[Route("api/[controller]")]
public class SurveyController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<SurveyController> _logger;

    public SurveyController(IMediator mediator, ILogger<SurveyController> logger) : base(mediator, logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("/api/Survey")]
    [ProducesResponseType(typeof(EmptyResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SaveSurveyAsync([FromBody] SaveSurveyCommand command)
    {
        return await SendRequestAsync(command);
    }
}
