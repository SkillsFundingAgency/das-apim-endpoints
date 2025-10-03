using MediatR;
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
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewById;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewByVacancyReferenceAndCandidateId;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByIds;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReferenceAndStatus;
using SFA.DAS.Recruit.Enums;

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

        [HttpGet]
        [Route("vacancyReference/{vacancyReference:long}/candidate/{candidateId:guid}")]
        public async Task<IActionResult> GetByVacancyReferenceAndCandidateId(
            [FromRoute, Required] long vacancyReference,
            [FromRoute, Required] Guid candidateId,
            CancellationToken token = default)
        {
            try
            {
                var result = await mediator.Send(new GetApplicationReviewByVacancyReferenceAndCandidateIdQuery(vacancyReference, candidateId), token);

                if (result.ApplicationReview == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting application review by vacancy reference and candidate id");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("vacancyReference/{vacancyReference:long}/status/{status}")]
        public async Task<IActionResult> GetByVacancyReferenceAndStatus(
            [FromRoute, Required] long vacancyReference,
            [FromRoute, Required] ApplicationReviewStatus status,
            [FromQuery] bool includeTemporaryStatus = false,
            CancellationToken token = default)
        {
            try
            {
                var result = await mediator.Send(new GetApplicationReviewsByVacancyReferenceAndStatusQuery(vacancyReference, status, includeTemporaryStatus), token);

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting application review by vacancy reference and status");
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

        [HttpPost]
        [Route("ManyByApplicationIds")]
        public async Task<IActionResult> GetManyByApplicationIds(
            [FromBody] GetManyApplicationReviewsApiRequest request,
            CancellationToken token = default)
        {
            try
            {
                var result = await mediator.Send(new GetApplicationReviewsByIdsQuery(request.ApplicationReviewIds), token);

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting application reviews by given Ids");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
