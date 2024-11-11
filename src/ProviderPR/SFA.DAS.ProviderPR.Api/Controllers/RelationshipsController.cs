using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderPR.Application.Queries.GetRelationship;
using SFA.DAS.ProviderPR.Application.Queries.GetRelationshipByEmail;
using SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationships;
using SFA.DAS.ProviderPR.InnerApi.Requests;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RelationshipsController(IMediator _mediator) : ControllerBase
{
    [HttpGet("{ukprn:long}")]
    [ProducesResponseType(typeof(GetProviderRelationshipsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelationships([FromRoute] long ukprn,
        [FromQuery] GetProviderRelationshipsRequest request, CancellationToken cancellationToken)
    {
        var query = new GetRelationshipsQuery(ukprn, request);

        GetProviderRelationshipsResponse result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("employeraccount/email/{email}")]
    [ProducesResponseType(typeof(GetRelationshipByEmailQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelationshipByEmail([FromRoute] string email, [FromQuery] long ukprn,
        CancellationToken cancellationToken)
    {
        var query = new GetRelationshipByEmailQuery(email, ukprn);

        GetRelationshipByEmailQueryResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(GetRelationshipResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelationship([FromQuery] long ukprn, [FromQuery] long accountLegalEntityId, CancellationToken cancellationToken)
    {
        GetRelationshipQuery query = new(ukprn, accountLegalEntityId);

        GetRelationshipResponse result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
