using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReview;

namespace SFA.DAS.RecruitQa.Api.Controllers;

[ApiController]
public class VacancyReviewController(IMediator mediator, ILogger<VacancyReviewController> logger) : ControllerBase
{
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
}