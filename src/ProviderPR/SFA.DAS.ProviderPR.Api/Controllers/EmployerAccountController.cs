using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationshipsByUkprnPayeAorn;

namespace SFA.DAS.ProviderPR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployerAccountController(IMediator _mediator, ILogger<EmployerAccountController> _logger) : ControllerBase
{
    [HttpGet("")]
    [ProducesResponseType(typeof(GetRelationshipsByUkprnPayeAornResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelationshipByUkprnAornPaye([FromQuery] long ukprn, [FromQuery] string aorn, [FromQuery] string encodedPaye, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get relationships  for ukprn {Ukprn}, aorn {aorn} and paye {encodedPaye}", ukprn, aorn, encodedPaye);

        var payeScheme = Uri.UnescapeDataString(encodedPaye);
        var query = new GetRelationshipsByUkprnPayeAornQuery(ukprn, aorn, payeScheme);
        GetRelationshipsByUkprnPayeAornResult result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
