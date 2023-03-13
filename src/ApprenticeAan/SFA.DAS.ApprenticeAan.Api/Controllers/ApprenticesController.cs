using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ApprenticesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApprenticesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("account/{apprenticeId}")]
    [ProducesResponseType(typeof(GetApprenticeAccountQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccount(Guid apprenticeId, CancellationToken cancellationToken)
    {
        var apprenticeAccountDetails = await _mediator.Send(new GetApprenticeAccountQuery { ApprenticeId = apprenticeId }, cancellationToken);
        if (apprenticeAccountDetails == null) return NotFound();
        return Ok(apprenticeAccountDetails);
    }
}
