using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeship.Queries.GetMyApprenticeship;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MyApprenticeshipController : ControllerBase
{
    private readonly IMediator _mediator;

    public MyApprenticeshipController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{apprenticeId}")]
    [ProducesResponseType(typeof(MyApprenticeship), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyApprenticeship(Guid apprenticeId, CancellationToken cancellationToken)
    {
        var myApprenticeship = await _mediator.Send(new GetMyApprenticeshipQuery { ApprenticeId = apprenticeId }, cancellationToken);

        if (myApprenticeship == null) return NotFound();

        return Ok(myApprenticeship);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateMyApprenticeship([FromBody] CreateMyApprenticeshipCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}