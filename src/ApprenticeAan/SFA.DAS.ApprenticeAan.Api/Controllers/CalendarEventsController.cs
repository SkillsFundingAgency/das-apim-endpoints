using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarEventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CalendarEventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType(typeof(GetCalendarEventsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCalendarEvents([FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetCalendarEventsQuery(requestedByMemberId), cancellationToken);
        return Ok(response);
    }
}
