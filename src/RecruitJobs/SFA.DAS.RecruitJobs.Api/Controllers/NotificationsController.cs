using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.RecruitJobs.Api.Models;
using SFA.DAS.RecruitJobs.Domain;
using SFA.DAS.RecruitJobs.InnerApi.Requests.Notifications;
using SFA.DAS.RecruitJobs.InnerApi.Responses.Notifications;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using NotificationEmail = SFA.DAS.RecruitJobs.InnerApi.Responses.DelayedNotifications.NotificationEmail;

namespace SFA.DAS.RecruitJobs.Api.Controllers;

[ApiController, Route("[controller]")]
public class NotificationsController: ControllerBase
{
    [HttpPost, Route("send")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> SendOne(
        [FromServices] INotificationService notificationService,
        [FromBody, Required] NotificationEmail email)
    {
        var command = new SendEmailCommand(email.TemplateId.ToString(), email.RecipientAddress, email.Tokens);
        await notificationService.Send(command);
        return TypedResults.NoContent();
    }
    
    [HttpPost, Route("create/vacancies/{vacancyId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> CreateVacancyNotifications(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromRoute] Guid vacancyId,
        CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.PostWithResponseCode<PostCreateVacancyNotificationsResponse>(
            new PostCreateVacancyNotificationsRequest(vacancyId));

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return TypedResults.NotFound();
        }

        return response.StatusCode.IsSuccessStatusCode()
            ? TypedResults.Ok(new DataResponse<List<NotificationEmail>>(response.Body))
            : TypedResults.Problem();
    }
    
    [HttpPost, Route("{status}/create/vacancies/{vacancyId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> CreateVacancyNotificationsByStatus(
        [FromServices] IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        [FromRoute] VacancyStatus status,
        [FromRoute] Guid vacancyId,
        CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.PostWithResponseCode<PostCreateVacancyNotificationsResponse>(
            new PostCreateVacancyNotificationsByStatusRequest(vacancyId, status));

        if (response.StatusCode is HttpStatusCode.NotFound)
        {
            return TypedResults.NotFound();
        }

        return response.StatusCode.IsSuccessStatusCode()
            ? TypedResults.Ok(new DataResponse<List<NotificationEmail>>(response.Body))
            : TypedResults.Problem();
    }
}