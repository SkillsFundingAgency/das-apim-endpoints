using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.Relationships.Queries.GetRelationships;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class RelationshipsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetRelationshipsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRelationships([FromQuery] GetRelationshipsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
