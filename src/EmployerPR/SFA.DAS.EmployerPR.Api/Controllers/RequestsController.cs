using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptPermissionsRequest;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclineAddAccountRequest;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinePermissionsRequest;
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
    public async Task<IActionResult> DeclinePermissionsRequest([FromRoute] Guid requestId, [FromBody] DeclinedRequestModel model, CancellationToken cancellationToken)
    {
        DeclinePermissionsRequestCommand command = new DeclinePermissionsRequestCommand()
        {
            ActionedBy = model.ActionedBy,
            RequestId = requestId
        };

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{requestId:guid}/permission/accepted")]
    [ProducesResponseType(typeof(GetRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AcceptPermissionsRequest([FromRoute] Guid requestId, [FromBody] AcceptPermissionsRequestModel model, CancellationToken cancellationToken)
    {
        AcceptPermissionsRequestCommand command = new AcceptPermissionsRequestCommand()
        {
            ActionedBy = model.ActionedBy,
            RequestId = requestId
        };

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{requestId:guid}/addaccount/declined")]
    [ProducesResponseType(typeof(GetRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeclineAddAccountRequest([FromRoute] Guid requestId, [FromBody] DeclinedRequestModel model, CancellationToken cancellationToken)
    {
        DeclineAddAccountRequestCommand command = new DeclineAddAccountRequestCommand()
        {
            ActionedBy = model.ActionedBy,
            RequestId = requestId
        };

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }
}
