using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinedRequest;
using SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;
using SFA.DAS.EmployerPR.Infrastructure;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestsController(IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient, IMediator _mediator) : ControllerBase
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

    [HttpPost("{requestId:guid}/permission/declined")]
    [ProducesResponseType(typeof(GetRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeclinePermissionRequest([FromRoute] Guid requestId, [FromBody] DeclinedRequestModel model, CancellationToken cancellationToken)
    {
        DeclinePermissionRequestCommand command = new DeclinePermissionRequestCommand()
        {
            ActionedBy = model.ActionedBy,
            RequestId = requestId
        };

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }
}
