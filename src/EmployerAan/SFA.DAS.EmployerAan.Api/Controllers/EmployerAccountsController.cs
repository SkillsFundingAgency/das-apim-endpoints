using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.User.GetUserAccounts;

namespace SFA.DAS.EmployerAan.Api.Controllers;

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
    [Route("{userId}/accounts")]
    public async Task<IActionResult> GetUserAccounts(string userId, [FromQuery] string email)
    {
        var result = await _mediator.Send(new GetUserAccountsQuery(userId, email));

        return Ok(result);
    }
}
