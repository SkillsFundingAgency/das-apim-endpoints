using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.AccountLegalEntities.Queries.GetAccountLegalEntities;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class EmployerAccountsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Route("{accountId}/LegalEntities")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetAccountLegalEntitiesQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccountLegalEntities(long accountId, CancellationToken cancellationToken)
    {
        var queryResult = await _mediator.Send(new GetAccountLegalEntitiesQuery { AccountId = accountId }, cancellationToken);
        return Ok(queryResult);
    }
}
