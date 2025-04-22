using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.User.GetUserAccounts;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[Obsolete("Use referenced shared controller action in Employer.Gov.Auth project AccountUsers/{userId}/accounts?email={email}")]
[ApiController]
[Route("[controller]")]
public class EmployerAccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployerAccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> GetUserAccounts([FromRoute] string userId, [FromQuery] string email, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUserAccountsQuery(userId, email), cancellationToken);

        return Ok(result);
    }
}
