using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.MemberNotificationSettings.Queries.GetMemberNotificationSettings;
using SFA.DAS.EmployerAan.Application.Settings.Commands;
using SFA.DAS.EmployerAan.Models.ApiRequests.Settings;

namespace SFA.DAS.EmployerAan.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MemberNotificationSettingsController : ControllerBase
{
    private readonly IMediator _mediator;
    public MemberNotificationSettingsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{memberId}")]
    [ProducesResponseType(typeof(GetMemberNotificationSettingsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMemberNotificationEventFormats(
        [FromRoute] Guid memberId,
        CancellationToken cancellationToken)
    {
        var memberNotificationSettings = await _mediator.Send(new GetMemberNotificationSettingsQuery(memberId), cancellationToken);

        return Ok(memberNotificationSettings);
    }

    [HttpPost("{memberId}")]
    public async Task<IActionResult> Post([FromRoute] Guid memberId, [FromBody] NotificationsSettingsApiRequest request)
    {
        await _mediator.Send(new UpdateNotificationSettingsCommand
        {
            MemberId = memberId,
            ReceiveNotifications = request.ReceiveNotifications,
            EventTypes = request.EventTypes.Select(x => new UpdateNotificationSettingsCommand.NotificationEventType
            {
                EventType = x.EventType,
                Ordering = x.Ordering,
                ReceiveNotifications = x.ReceiveNotifications
            }).ToList(),
            Locations = request.Locations.Select(x => new UpdateNotificationSettingsCommand.Location
            {
                Name = x.Name,
                Radius = x.Radius,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToList()
        });

        return Ok();
    }
}