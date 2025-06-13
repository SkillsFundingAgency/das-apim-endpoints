﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReference;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationReviewsController(IMediator mediator,
        ILogger<ApplicationReviewsController> logger) : ControllerBase
    {
        [HttpGet]
        [Route("{vacancyReference:long}")]
        public async Task<IActionResult> GetApplicationReviewsByVacancyReference(
            [FromRoute, Required] long vacancyReference,
            CancellationToken token = default)
        {
            try
            {
                var result = await mediator.Send(new GetApplicationReviewsByVacancyReferenceQuery(vacancyReference), token);

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting application reviews");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateApplicationReview(
            [FromRoute, Required] Guid id,
            [FromBody] PostApplicationReviewApiRequest request,
            CancellationToken token = default)
        {
            try
            {
                await mediator.Send(new PatchApplicationReviewCommand(id,
                    request.Status,
                    request.TemporaryReviewStatus,
                    request.EmployerFeedback,
                    request.HasEverBeenEmployerInterviewing,
                    request.DateSharedWithEmployer), token);

                return NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error posting application review");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
