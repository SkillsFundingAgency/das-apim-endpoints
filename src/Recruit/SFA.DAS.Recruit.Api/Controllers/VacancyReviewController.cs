using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Api.Models.VacancyReviews;
using SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;
using SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReview;
using SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReviewsByFilter;
using SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReviewsByVacancyReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
public class VacancyReviewController(
    IMediator mediator,
    ILogger<VacancyReviewController> logger) : ControllerBase
{

    [HttpGet]
    [Route("[controller]s")]
    public async Task<IActionResult> Get(
        [FromQuery] List<string>? reviewStatus,
        [FromQuery] DateTime? expiredAssignationDateTime)
    {
        try
        {
            var result = await mediator.Send(new GetVacancyReviewsByFilterQuery
            {
                Status = reviewStatus,
                ExpiredAssignationDateTime = expiredAssignationDateTime
            });

            var dtoList = result.VacancyReviews.Select(x => (VacancyReviewDto)x).ToList();
            return Ok(new GetVacancyReviewsApiResponse { VacancyReviews = dtoList });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while getting vacancy reviews by filter");
            return new StatusCodeResult(500);
        }
    }

    [HttpGet]
    [Route("[controller]s/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            var result = await mediator.Send(new GetVacancyReviewQuery { Id = id });
            if (result.VacancyReview == null)
            {
                return NotFound();
            }

            return Ok(new GetVacancyReviewApiResponse { VacancyReview = (VacancyReviewDto)result.VacancyReview });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while getting vacancy review by Id");
            return new StatusCodeResult(500);
        }
    }

    [HttpGet]
    [Route("vacancies/{vacancyReference:long}/vacancyReviews")]
    public async Task<IActionResult> GetByVacancyReference(
        [FromRoute] long vacancyReference,
        [FromQuery] string? status,
        [FromQuery] List<string>? manualOutcome,
        [FromQuery] bool includeNoStatus)
    {
        try
        {
            var result = await mediator.Send(new GetVacancyReviewsByVacancyReferenceQuery
            {
                VacancyReference = vacancyReference,
                Status = status,
                ManualOutcome = manualOutcome,
                IncludeNoStatus = includeNoStatus
            });

            var dtoList = result.VacancyReviews.Select(x => (VacancyReviewDto)x).ToList();
            return Ok(new GetVacancyReviewsApiResponse { VacancyReviews = dtoList });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while getting vacancy reviews by vacancy reference");
            return new StatusCodeResult(500);
        }
    }

    [HttpPost]
    [Route("[controller]s/{id:guid}")]
    public async Task<IActionResult> UpsertVacancyReview(
        [FromRoute] Guid id,
        [FromBody] VacancyReviewDto vacancyReview)
    {
        try
        {
            await mediator.Send(new UpsertVacancyReviewCommand
            {
                VacancyReview = (InnerApi.Recruit.Requests.VacancyReviewDto)vacancyReview,
                Id = id
            });
            return Created();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while upserting vacancy review");
            return new StatusCodeResult(500);
        }
    }
}