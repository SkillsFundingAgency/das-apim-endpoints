using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.Queries.GetRequest;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.Api.Controllers;

public class RequestsController(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : ControllerBase
{
    [HttpGet("{requestId:guid}")]
    [ProducesResponseType(typeof(GetRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRequest([FromRoute] Guid requestId, CancellationToken cancellationToken)
    {
        GetRequestResponse? result = await _providerRelationshipsApiRestClient.GetRequest(requestId, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
