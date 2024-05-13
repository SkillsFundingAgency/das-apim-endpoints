using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.Queries.GetAccountLegalEntities;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class EmployerAccountsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Route("{hashedAccountId}/legalentities")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetAccountLegalEntitiesQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccountLegalEntities(string hashedAccountId, CancellationToken cancellationToken)
    {
        var queryResult = await _mediator.Send(new GetAccountLegalEntitiesQuery { HashedAccountId = hashedAccountId }, cancellationToken);
        return Ok(queryResult);
    }
}
