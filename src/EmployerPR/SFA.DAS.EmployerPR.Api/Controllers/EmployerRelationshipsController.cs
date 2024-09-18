using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[ApiController]
[Route("relationships")]
public class EmployerRelationshipsController(IMediator _mediator) : ControllerBase
{
    [HttpGet("{accountId}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetEmployerRelationshipsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployerRelationships([FromRoute] long accountId, CancellationToken cancellationToken)
    {
        GetEmployerRelationshipsQuery query = new(accountId);

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}