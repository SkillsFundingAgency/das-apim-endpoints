using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.RecruitJobs.Api.Models.Mappers;
using SFA.DAS.RecruitJobs.InnerApi.Requests.DelayedNotifications;
using SFA.DAS.RecruitJobs.InnerApi.Responses.DelayedNotifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[ApiController, Route("delayed-notifications/")]
public class DelayedNotificationsController: ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<NotificationEmail>), StatusCodes.Status200OK)]
    public async Task<IResult> GetBatchByDate(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromQuery, Required] DateTime? dateTime)
    {
        var results = await recruitApiClient.Get<GetDelayedNotificationsByDateResponse>(new GetDelayedNotificationsByDateRequest(dateTime!.Value));
        return results is null
            ? Results.NotFound()
            : TypedResults.Ok(results.Emails.ToGetResponse());
    }
    
    [HttpPost, Route("delete")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> DeleteMany(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromBody, Required] IEnumerable<long> ids)
    {
        var results = await recruitApiClient.DeleteWithResponseCode<NullResponse>(new DeleteNotificationsByIdsRequest(ids));
        return Results.StatusCode((int)results.StatusCode);
    }
    
    [HttpPost, Route("send")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> SendOne(
        [FromServices] INotificationService notificationService,
        [FromBody, Required] NotificationEmail email)
    {
        var command = new SendEmailCommand(email.TemplateId.ToString(), email.RecipientAddress, email.Tokens);
        await notificationService.Send(command);
        return Results.NoContent();
    }
}