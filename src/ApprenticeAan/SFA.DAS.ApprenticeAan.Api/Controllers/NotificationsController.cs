using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Notifications;
using System.Net;

namespace SFA.DAS.ApprenticeAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationsController : Controller
{
    private readonly IAanHubRestApiClient _outerApiClient;

    public NotificationsController(IAanHubRestApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetNotificationResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNotification([FromRoute] Guid id, [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)] Guid requestedByMemberId, CancellationToken cancellationToken)
    {
        var response = await _outerApiClient.GetNotification(id, requestedByMemberId, cancellationToken);
        var result = response.ResponseMessage.StatusCode switch
        {
            HttpStatusCode.OK => response.GetContent(),
            HttpStatusCode.NotFound => null,
            _ => throw new InvalidOperationException("Get notification didn't come back with a successful response")
        };
        return Ok(result);
    }
}
