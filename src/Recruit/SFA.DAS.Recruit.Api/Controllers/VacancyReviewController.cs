using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
public class VacancyReviewController(IMediator mediator, ILogger<VacancyReviewController> logger) : ControllerBase
{
    [HttpPost]
    [Route("[controller]s/{id}")]
    public async Task<IActionResult> UpsertVacancyReview([FromRoute] Guid id, [FromBody] VacancyReviewDto vacancyReview)
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