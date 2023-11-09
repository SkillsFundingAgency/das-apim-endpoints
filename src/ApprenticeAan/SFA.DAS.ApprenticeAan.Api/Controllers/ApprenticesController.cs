using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Apprentices;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[Route("[controller]")]
public class ApprenticesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAanHubRestApiClient _apiClient;

    public ApprenticesController(IMediator mediator, IAanHubRestApiClient apiClient)
    {
        _mediator = mediator;
        _apiClient = apiClient;
    }

    [HttpGet]
    [Route("{apprenticeId}/account")]
    [ProducesResponseType(typeof(GetApprenticeAccountQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccount(Guid apprenticeId, CancellationToken cancellationToken)
    {
        var apprenticeAccountDetails = await _mediator.Send(new GetApprenticeAccountQuery { ApprenticeId = apprenticeId }, cancellationToken);
        if (apprenticeAccountDetails == null) return NotFound();
        return Ok(apprenticeAccountDetails);
    }

    [HttpGet]
    [Route("{apprenticeId}")]
    [ProducesResponseType(typeof(GetApprenticeResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetApprentice(Guid apprenticeId, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetApprenticeMember(apprenticeId, cancellationToken);

        if (response.ResponseMessage.StatusCode == HttpStatusCode.NotFound) return NotFound();

        return Ok(response.GetContent());
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateApprenticeMemberCommandResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateApprenticeMember([FromBody] CreateApprenticeMemberCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
