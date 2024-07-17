using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderPR.Application.Queries.GetEasUserByEmail;
using SFA.DAS.ProviderPR.Application.Queries.GetRelationships;

namespace SFA.DAS.ProviderPR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RelationshipsController(IMediator _mediator, ILogger<RelationshipsController> _logger) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(GetRelationshipsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelationships(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get relationships invoked");
        GetRelationshipsQueryResult result = await _mediator.Send(new GetRelationshipsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("employeraccount/email/{email}")]
    [ProducesResponseType(typeof(GetEasUserByEmailQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEasUserByEmail(string email, [FromQuery] long ukprn, CancellationToken cancellationToken)
    {
        var query = new GetEasUserByEmailQuery(email, ukprn);
        GetEasUserByEmailQueryResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
