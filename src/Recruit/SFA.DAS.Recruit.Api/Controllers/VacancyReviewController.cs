using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Services;
using SFA.DAS.Recruit.Application.VacancyReview.Commands.UpsertVacancyReview;
using SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReview;
using SFA.DAS.Recruit.Domain.Vacancy;

namespace SFA.DAS.Recruit.Api.Controllers;

[ApiController]
public class VacancyReviewController(IMediator mediator, ILogger<EmployerAccountsController> logger) : ControllerBase
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

    [HttpPost]
    [Route("[controller]s/{id}")]
    public async Task<IActionResult> UpsertVacancyReview(
        [FromRoute] Guid id,
        [FromBody] VacancyReviewDto vacancyReview,
        [FromServices] IRecruitArtificialIntelligenceService aiService)
    {
        try
        {
            await mediator.Send(new UpsertVacancyReviewCommand
            {
                VacancyReview = (InnerApi.Recruit.Requests.VacancyReviewDto)vacancyReview,
                Id = id
            });

            if (vacancyReview.EnableAiProcessing is null)
            {
                logger.LogInformation("Ai not specified in request");
            }
            
            if (vacancyReview.EnableAiProcessing is true)
            {
                logger.LogInformation("Ai is enabled");
                HttpContext.Response.OnCompleted(async () =>
                {
                    try
                    {
                        var vacancy = JsonSerializer.Deserialize<VacancySnapshot>(vacancyReview.VacancySnapshot, Global.JsonSerializerOptionsCaseInsensitive);
                        logger.LogInformation("Vacancy has deserialized");
                        await aiService.SendVacancyReviewAsync(vacancy, CancellationToken.None);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "An unhandled exception occurred whilst processing the VacancyReview AI call");
                    }
                });
            }
            else
            {
                logger.LogInformation("Ai is not enabled");
            }
            return Created();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occured while upserting vacancy review");
            return new StatusCodeResult(500);
        }
    }
}