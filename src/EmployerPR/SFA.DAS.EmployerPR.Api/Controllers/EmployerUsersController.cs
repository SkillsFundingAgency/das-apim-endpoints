using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.AccountLegalEntities.Queries.GetAccountLegalEntities;
using SFA.DAS.EmployerPR.Application.UserAccounts.Queries.GetUserAccounts;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class EmployerUsersController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetAccountLegalEntitiesQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserAccounts([FromQuery] string userId, [FromQuery] string email, CancellationToken cancellationToken)
    {
        GetUserAccountsQuery query = new(userId, email);
        GetUserAccountsQueryResult result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
