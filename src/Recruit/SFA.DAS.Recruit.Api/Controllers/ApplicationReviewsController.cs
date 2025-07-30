﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewById;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReference;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetPagedApplicationReviewsByVacancyReference;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationReviewsController(IMediator mediator,
        ILogger<ApplicationReviewsController> logger) : ControllerBase
    {
        [HttpGet]
        [Route("vacancyReference/{vacancyReference:long}")]
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

        [HttpGet]
        [Route("vacancyReference/paginated/{vacancyReference:long}")]
        public async Task<IActionResult> GetPagedByVacancyReference(
            [FromRoute, Required] long vacancyReference,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sortColumn = "CreatedDate",
            [FromQuery] bool isAscending = false,
            CancellationToken token = default)
        {
            try
            {
                var result = await mediator.Send(new GetPagedApplicationReviewsByVacancyReferenceQuery(vacancyReference,
                        pageNumber,
                        pageSize,
                        sortColumn,
                        isAscending),
                    token);

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting paged application reviews");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> Get(
            [FromRoute, Required] Guid id,
            CancellationToken token = default)
        {
            try
            {
                var result = await mediator.Send(new GetApplicationReviewByIdQuery(id), token);

                if (result.ApplicationReview == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting application review by id");
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
