using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Commands.UpsertVacancyReview;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReview;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByAccountLegalEntity;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByFilter;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByUser;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByVacancyReference;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByUser;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewSummary;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByAccountLegalEntity;

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
    
    [HttpPost]
    [Route("[controller]s/{id}")]
    public async Task<IActionResult> UpsertVacancyReview([FromRoute] Guid id, [FromBody] VacancyReviewDto vacancyReview)
    {
        try
        {
            await mediator.Send(new UpsertVacancyReviewCommand
            {
                VacancyReview = (SFA.DAS.RecruitQa.InnerApi.Requests.VacancyReviewDto)vacancyReview,
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
    [Route("vacancies/{vacancyReference}/vacancyreviews")]
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

    [HttpGet]
    [Route("users/VacancyReviews/count")]
    public async Task<IActionResult> GetCountByUser([FromQuery] string userEmail, [FromQuery] bool? approvedFirstTime, [FromQuery] DateTime? assignationExpiry)
    {
        try
        {
            var result = await mediator.Send(new GetVacancyReviewsCountByUserQuery
            {
                UserEmail = userEmail,
                ApprovedFirstTime = approvedFirstTime,
                AssignationExpiry = assignationExpiry
            });

            return Ok(new GetVacancyReviewsCountApiResponse { Count = result.Count });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while getting vacancy reviews count by user");
            return new StatusCodeResult(500);
        }
    }

    [HttpGet]
    [Route("[controller]s/summary")]
    public async Task<IActionResult> GetSummary()
    {
        try
        {
            var result = await mediator.Send(new GetVacancyReviewSummaryQuery());
            var model = new GetVacancyReviewSummaryApiResponse
            {
                TotalVacanciesForReview = result.TotalVacanciesForReview,
                TotalVacanciesResubmitted = result.TotalVacanciesResubmitted,
                TotalVacanciesBrokenSla = result.TotalVacanciesBrokenSla,
                TotalVacanciesSubmittedTwelveTwentyFourHours = result.TotalVacanciesSubmittedTwelveTwentyFourHours
            };
            return Ok(model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while getting vacancy review summary");
            return new StatusCodeResult(500);
        }
    }

    [HttpGet]
    [Route("users/VacancyReviews")]
    public async Task<IActionResult> GetByUser([FromQuery] string userId, [FromQuery] DateTime? assignationExpiry, [FromQuery] string? status)
    {
        try
        {
            var result = await mediator.Send(new GetVacancyReviewsByUserQuery
            {
                UserId = userId,
                AssignationExpiry = assignationExpiry,
                Status = status
            });

            var dtoList = result.VacancyReviews.Select(x => (VacancyReviewDto)x).ToList();
            return Ok(new GetVacancyReviewsApiResponse { VacancyReviews = dtoList });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while getting vacancy reviews by user");
            return new StatusCodeResult(500);
        }
    }

    [HttpGet]
    [Route("accounts/{accountLegalEntityId}/vacancyreviews/count")]
    public async Task<IActionResult> GetCountByAccountLegalEntity(
        [FromRoute] long accountLegalEntityId,
        [FromQuery] string? status,
        [FromQuery] string? manualOutcome,
        [FromQuery] string? employerNameOption
        )
    {
        try
        {
            var result = await mediator.Send(new GetVacancyReviewsCountByAccountLegalEntityQuery
            {
                AccountLegalEntityId = accountLegalEntityId,
                Status = status,
                ManualOutcome = manualOutcome,
                EmployerNameOption =employerNameOption
            });

            return Ok(new GetVacancyReviewsCountApiResponse { Count = result.Count });
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while getting vacancy reviews count by account legal entity");
            return new StatusCodeResult(500);
        }
    }
}