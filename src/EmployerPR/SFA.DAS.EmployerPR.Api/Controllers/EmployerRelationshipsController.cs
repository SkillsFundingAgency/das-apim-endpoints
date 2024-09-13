using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.EmployerRelationships.Queries.GetEmployerRelationships;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[ApiController]
[Route("relationships")]
public class EmployerRelationshipsController(IMediator _mediator) : ControllerBase
{
    [HttpGet("employeraccount/{accountHashedId}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetEmployerRelationshipsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployerRelationships(string accountHashedId, [FromQuery] long? ukprn, [FromQuery] string? accountlegalentityPublicHashedId, CancellationToken cancellationToken)
    {
        GetEmployerRelationshipsQuery query = new(accountHashedId, ukprn, accountlegalentityPublicHashedId);

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}