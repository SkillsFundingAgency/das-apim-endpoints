using System.Net;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.EmployerAan.InnerApi.Notifications;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly IAanHubRestApiClient _client;

    public NotificationsController(IAanHubRestApiClient client)
    {
        _client = client;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetNotificationResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotification([FromRoute] Guid id, [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, CancellationToken cancellationToken)
    {
        var response = await _client.GetNotification(id, requestedByMemberId, cancellationToken);
        var result = response.ResponseMessage.StatusCode switch
        {
            HttpStatusCode.OK => response.GetContent(),
            HttpStatusCode.NotFound => null,
            _ => throw new InvalidOperationException("Get notification didn't come back with a successful response")
        };
        return Ok(result);
    }
}
