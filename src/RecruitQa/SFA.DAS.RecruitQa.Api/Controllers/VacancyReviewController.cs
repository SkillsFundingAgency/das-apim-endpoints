using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReview;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByFilter;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByAccountLegalEntity;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByVacancyReference;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsCountByUser;

namespace SFA.DAS.RecruitQa.Api.Controllers;

[ApiController]
public class VacancyReviewController(IMediator mediator, ILogger<VacancyReviewController> logger) : ControllerBase
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
    [Route("accounts/{accountLegalEntityId}/vacancyreviews")]
    public async Task<IActionResult> GetByAccountLegalEntity([FromRoute] long accountLegalEntityId)
    {
        try
        {
            var result = await mediator.Send(new GetVacancyReviewsByAccountLegalEntityQuery
            {
                AccountLegalEntityId = accountLegalEntityId
            });

            var dtoList = result.VacancyReviews.Select(x => (VacancyReviewDto)x).ToList();
            return Ok(new GetVacancyReviewsApiResponse { VacancyReviews = dtoList });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while getting vacancy reviews by account legal entity");
            return new StatusCodeResult(500);
        }
    }

    [HttpGet]
    [Route("[controller]s/{vacancyReference}/vacancyreviews")]
    public async Task<IActionResult> GetByVacancyReference([FromRoute] long vacancyReference, [FromQuery] string status)
    {
        try
        {
            var result = await mediator.Send(new GetVacancyReviewsByVacancyReferenceQuery
            {
                VacancyReference = vacancyReference,
                Status = status
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

    [HttpGet]
    [Route("users/{userId}/VacancyReviews/count")]
    public async Task<IActionResult> GetCountByUser([FromRoute] string userId, [FromQuery] bool? approvedFirstTime)
    {
        try
        {
            var result = await mediator.Send(new GetVacancyReviewsCountByUserQuery
            {
                UserId = userId,
                ApprovedFirstTime = approvedFirstTime
            });

            return Ok(new GetVacancyReviewsCountApiResponse { Count = result.Count });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while getting vacancy reviews count by user");
            return new StatusCodeResult(500);
        }
    }
}