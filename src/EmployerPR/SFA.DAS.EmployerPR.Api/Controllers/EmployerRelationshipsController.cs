using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[ApiController]
[Route("relationships")]
public class EmployerRelationshipsController(IMediator _mediator) : ControllerBase
{
    [Produces("application/json")]
    [HttpGet("employeraccount/{accountId:long}")]
    [ProducesResponseType(typeof(GetEmployerRelationshipsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployerRelationships([FromRoute] long accountId, [FromQuery] long? ukprn, [FromQuery] string? accountlegalentityPublicHashedId, CancellationToken cancellationToken)
    {
        GetEmployerRelationshipsQuery query = new(accountId, ukprn, accountlegalentityPublicHashedId);

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
