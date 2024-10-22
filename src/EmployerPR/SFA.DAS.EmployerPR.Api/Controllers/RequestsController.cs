using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptAddAccountRequest;
using SFA.DAS.EmployerPR.Application.Requests.Commands.AcceptPermissionsRequest;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclineAddAccountRequest;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclineCreateAccountRequest;
using SFA.DAS.EmployerPR.Application.Requests.Commands.DeclinePermissionsRequest;
using SFA.DAS.EmployerPR.Application.Requests.Queries.GetRequest;
using SFA.DAS.EmployerPR.Application.Requests.Queries.ValidateRequest;
using SFA.DAS.EmployerPR.InnerApi.Responses;

namespace SFA.DAS.EmployerPR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestsController(IMediator _mediator) : ControllerBase
{
    [HttpGet("{requestId:guid}")]
    [ProducesResponseType(typeof(GetRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRequest([FromRoute] Guid requestId, CancellationToken cancellationToken)
    {
        GetRequestResponse? result = await _mediator.Send(new GetRequestQuery(requestId), cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost("{requestId:guid}/permission/declined")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    [HttpPost("{requestId:guid}/addaccount/accepted")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AcceptAddAccountRequest([FromRoute] Guid requestId, [FromBody] AcceptAddAccountRequestModel model, CancellationToken cancellationToken)
    {
        AcceptAddAccountRequestCommand command = new AcceptAddAccountRequestCommand()
        {
            ActionedBy = model.ActionedBy,
            RequestId = requestId
        };

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpGet("{requestId:guid}/createaccount/validate")]
    [ProducesResponseType(typeof(ValidatePermissionsRequestQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateRequest([FromRoute] Guid requestId, CancellationToken cancellationToken)
    {
        ValidatePermissionsRequestQuery query = new(requestId);
        ValidatePermissionsRequestQueryResult result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{requestId:guid}/createaccount/declined")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeclineCreateAccountRequest([FromRoute] Guid requestId, [FromBody] DeclinedRequestModel model, CancellationToken cancellationToken)
    {
        DeclineCreateAccountRequestCommand command = new DeclineCreateAccountRequestCommand()
        {
            ActionedBy = model.ActionedBy,
            RequestId = requestId
        };

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }
}
