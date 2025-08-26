using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.ApplicationReview.Events.ApplicationReviewShared;

namespace SFA.DAS.Recruit.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class EventsController(IPublisher mediator,
    ILogger<ApplicationReviewsController> logger) : ControllerBase
{
    [HttpPost]
    [Route("application-shared-with-employer")]
    public async Task<IActionResult> SendApplicationReviewSharedNotification(
        [FromBody] PostApplicationReviewSharedNotificationApiRequest request,
        CancellationToken token = default)
    {
        try
        {
            await mediator.Publish(new ApplicationReviewSharedEvent(request.HashAccountId,
                request.AccountId,
                request.VacancyId,
                request.ApplicationId,
                request.TrainingProvider,
                request.AdvertTitle,
                request.VacancyReference), token);
            return NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error posting application review shared with employer notification event");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}