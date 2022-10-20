using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgressInternal.Application.Commands.TrackProgress;

namespace SFA.DAS.TrackProgressInternal.Api.Controllers;

[ApiController]
public class CourseController : ControllerBase
{
    private readonly IMediator mediator;

    public CourseController(IMediator mediator) => this.mediator = mediator;

    public record PopulateKsbsRequest(Guid[] KsbIds);

    [HttpPost]
    [Route("/courses/{standard}/ksbs")]
    public async Task<IActionResult> PopulateKsbs(string standard, [FromBody] PopulateKsbsRequest request)
    {
        await mediator.Send(new PopulateKsbsCommand(standard, request.KsbIds));
        return Ok();
    }
}