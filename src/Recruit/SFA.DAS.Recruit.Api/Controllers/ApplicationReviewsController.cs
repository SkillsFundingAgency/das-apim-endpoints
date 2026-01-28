using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewById;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewByVacancyReferenceAndCandidateId;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByIds;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReference;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyReferenceAndTempStatus;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewStatsByVacancyReference;
using SFA.DAS.Recruit.Enums;
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
        [Route("vacancyReference/{vacancyReference:long}/temp-status/{status}")]
        public async Task<IActionResult> GetByVacancyReferenceAndStatus(
            [FromRoute, Required] long vacancyReference,
            [FromRoute, Required] ApplicationReviewStatus status,
            CancellationToken token = default)
        {
            try
            {
                var result = await mediator.Send(new GetApplicationReviewsByVacancyReferenceAndTempStatusQuery(vacancyReference, status), token);

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting application review by vacancy reference and temp status");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("vacancyReference/{vacancyReference:long}/count")]
        public async Task<IActionResult> GetStatusCountByVacancyReference(
            [FromRoute, Required] long vacancyReference,
            CancellationToken token = default)
        {
            try
            {
                var result = await mediator.Send(new GetApplicationReviewStatsByVacancyReferenceQuery(vacancyReference), token);

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error getting application reviews count by vacancy reference");
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
                    request.DateSharedWithEmployer,
                    request.CandidateFeedback), token);

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
