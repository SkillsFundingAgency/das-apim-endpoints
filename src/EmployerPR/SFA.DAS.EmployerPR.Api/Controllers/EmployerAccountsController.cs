using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.Queries.GetAccountLegalEntities;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class EmployerAccountsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Route("{accountHashedId}/LegalEntities")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetAccountLegalEntitiesQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccountLegalEntities(string accountHashedId, CancellationToken cancellationToken)
    {
        var queryResult = await _mediator.Send(new GetAccountLegalEntitiesQuery { AccountHashedId = accountHashedId }, cancellationToken);
        return Ok(queryResult);
    }
}
