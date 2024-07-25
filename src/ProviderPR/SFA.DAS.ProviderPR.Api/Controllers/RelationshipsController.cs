using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderPR.Application.Queries.GetRelationshipByEmail;
using SFA.DAS.ProviderPR.Application.Queries.GetRelationships;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Requests;
using SFA.DAS.ProviderPR.InnerApi.Responses;
using System.Text.Json;

namespace SFA.DAS.ProviderPR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RelationshipsController(IMediator _mediator, IProviderRelationshipsApiRestClient _prApiClient, ILogger<RelationshipsController> _logger) : ControllerBase
{
    [HttpGet("{ukprn:long}")]
    [ProducesResponseType(typeof(GetProviderRelationshipsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelationships([FromRoute] long ukprn, [FromQuery] GetProviderRelationshipsRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get relationships invoked for {Ukprn} with query parameters {Params}", ukprn, JsonSerializer.Serialize(request));

        GetProviderRelationshipsResponse result = await _prApiClient.GetProviderRelationships(ukprn, Request.QueryString.ToString(), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetRelationshipsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelationships(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get relationships invoked");
        GetRelationshipsQueryResult result = await _mediator.Send(new GetRelationshipsQuery(), cancellationToken);
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
}
