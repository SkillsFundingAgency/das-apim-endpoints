using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarEventsController : ControllerBase
{
    private readonly IMediator _mediator;
    public const string RequestedByMemberIdHeader = "X-RequestedByMemberId";

    public CalendarEventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("")]
    // [ProducesResponseType(typeof(GetCalendarEventsQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCalendarEvents([FromHeader(Name = RequestedByMemberIdHeader)] Guid requestedByMemberId, CancellationToken cancellationToken)
    {


        var query = new GetCalendarEventsQuery();


        var x = await _mediator.Send(query, cancellationToken);

        return NotFound();
    }
}
