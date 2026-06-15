using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

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
        var results = await recruitApiClient.Get<GetNotificationsBatchByDateResponse>(new GetNotificationsBatchByDateApiRequest(dateTime!.Value));
        return TypedResults.Ok(results?.Emails ?? []);
    }

    [HttpGet]
    [Route("users/inactive")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<NotificationEmail>), StatusCodes.Status200OK)]
    public async Task<IResult> GetBatchByUserInActiveStatus(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    {
        var results = await recruitApiClient.Get<GetNotificationsBatchByUserStatusResponse>(new GetNotificationsBatchByUserstatusApiRequest(UserStatus.Inactive));
        return TypedResults.Ok(results?.Emails ?? []);
    }
    
    [HttpPost, Route("delete")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> DeleteMany(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromBody, Required] IEnumerable<long> ids)
    {
        var results = await recruitApiClient.DeleteWithResponseCode<NullResponse>(new DeleteNotificationsApiRequest(ids.ToList()));
        return Results.StatusCode((int)results.StatusCode);
    }
    
    [HttpPost, Route("send")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> SendOne(
        [FromServices] INotificationService notificationService,
        [FromBody, Required] NotificationEmail email)
    {
        var command = new SendEmailCommand(email.TemplateId.ToString(), email.RecipientAddress, new ReadOnlyDictionary<string, string>(email.Tokens ?? new Dictionary<string, string>()));
        await notificationService.Send(command);
        return Results.NoContent();
    }
}