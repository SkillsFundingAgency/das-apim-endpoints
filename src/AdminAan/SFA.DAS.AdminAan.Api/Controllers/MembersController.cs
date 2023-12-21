using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Application.Members.GetMemberProfile;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class MembersController : ControllerBase
{
    private readonly IAanHubRestApiClient _apiClient;
    private readonly IMediator _mediator;

    public MembersController(IAanHubRestApiClient apiClient, IMediator mediator)
    {
        _apiClient = apiClient;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetMembersResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMembers([FromQuery] GetMembersRequest requestModel, CancellationToken cancellationToken)
    {
        var response = await _apiClient.GetMembers(Request.QueryString.ToString(), cancellationToken);
        return Ok(response);
    }

    [HttpGet("{memberId}/profile")]
    [ProducesResponseType(typeof(GetMemberProfileQueryResult), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetMemberProfile(
        [FromRoute] Guid memberId,
        [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        GetMemberProfileQuery query = new(memberId, requestedByMemberId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
