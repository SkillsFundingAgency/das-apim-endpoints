using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AccountInvitation;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccount;
using SFA.DAS.ProviderPR.Application.Requests.Commands.CreatePermissions;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestsController(IMediator _mediator, IProviderRelationshipsApiRestClient _providerRelationshipsApiRestClient) : ControllerBase
{
    [HttpPost("addaccount")]
    [ProducesResponseType(typeof(AddAccountRequestCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAccount([FromBody] AddAccountRequestCommand command, CancellationToken cancellationToken)
    {
        AddAccountRequestCommandResult result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("createaccount")]
    [ProducesResponseType(typeof(CreateAccountInvitationRequestCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountInvitationRequestCommand command, CancellationToken cancellationToken)
    {
        CreateAccountInvitationRequestCommandResult result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

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

    [HttpGet]
    [ProducesResponseType(typeof(GetRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRequest([FromQuery] long ukprn, [FromQuery] string? paye, [FromQuery] string? email, [FromQuery] long? accountLegalEntityId, CancellationToken cancellationToken)
    {
        GetRequestResponse? result = await _providerRelationshipsApiRestClient.GetRequest(ukprn, paye, email, accountLegalEntityId, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    [HttpPost("permission")]
    [ProducesResponseType(typeof(CreatePermissionRequestCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePermissions([FromBody] CreatePermissionRequestCommand command, CancellationToken cancellationToken)
    {
        CreatePermissionRequestCommandResult result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
