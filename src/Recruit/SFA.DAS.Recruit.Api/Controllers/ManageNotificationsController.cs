using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Models.ManageUserNotificationPreferences;
using SFA.DAS.Recruit.Application.User.Commands.UpdateUserNotificationPreferences;
using SFA.DAS.Recruit.Application.User.Queries.GetUserByDfeUserId;
using SFA.DAS.Recruit.Application.User.Queries.GetUserByIdamsId;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
[Route("[controller]/")]
public class ManageNotificationsController(IMediator mediator) : ControllerBase
{
    [HttpGet, Route("employer/{idamsId}")]
    public async Task<IActionResult> GetUserNotificationPreferencesByIdams([FromRoute] string idamsId)
    {
        var result = await mediator.Send(new GetUserByIdamsIdQuery(idamsId));
        if (result?.User is null)
        {
            return NotFound();
        }

        return Ok(new GetUserNotificationPreferencesByIdamsIdResponse
        {
            Id = result.User.Id,
            IdamsId = result.User.IdamsUserId,
            NotificationPreferences = result.User.NotificationPreferences,
        });
    }
    
    [HttpGet, Route("provider/{dfeUserId}")]
    public async Task<IActionResult> GetUserNotificationPreferencesByDfeUserId([FromRoute] string dfeUserId)
    {
        var result = await mediator.Send(new GetUserByDfeUserIdQuery(dfeUserId));
        if (result?.User is null)
        {
            return NotFound();
        }

        return Ok(new GetUserNotificationPreferencesByDfeUserIdResponse
        {
            Id = result.User.Id,
            DfeUserId = result.User.DfEUserId,
            NotificationPreferences = result.User.NotificationPreferences,
        });
    }

    [HttpPost, Route("{id:guid}")]
    public async Task<IActionResult> PostUpdateUserNotifications([FromRoute] Guid id, [FromBody] NotificationPreferences notificationPreferences)
    {
        var result = await mediator.Send(new UpdateUserNotificationPreferencesCommand(id, notificationPreferences));
        return result 
            ? Ok()
            : NotFound();
    }
}