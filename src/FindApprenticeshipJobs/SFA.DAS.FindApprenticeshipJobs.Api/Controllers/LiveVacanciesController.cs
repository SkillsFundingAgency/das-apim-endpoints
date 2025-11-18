using System.ComponentModel.DataAnnotations;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LiveVacanciesController(IMediator mediator,
    ILogger<LiveVacanciesController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] uint pageSize, [FromQuery] uint pageNo, [FromQuery] DateTime? closingDate, CancellationToken cancellationToken)
    {
        logger.LogInformation("Get Live Vacancies invoked");

        try
        {
            var result = await mediator.Send(new GetLiveVacanciesQuery
            {
                PageNumber = (int)pageNo,
                PageSize = (int)pageSize,
                ClosingDate = closingDate
            }, cancellationToken);
            var viewModel = (GetLiveVacanciesApiResponse)result;
            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error invoking Get Live Vacancies");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{vacancyReference}")]
    public async Task<IActionResult> GetLiveVacancy([FromRoute] long vacancyReference, CancellationToken cancellationToken)
    {
        logger.LogInformation("Get Live Vacancy invoked - vacancy reference: {VacancyReference}", vacancyReference);

        try
        {
            var result = await mediator.Send(new GetLiveVacancyQuery { VacancyReference = vacancyReference }, cancellationToken);
            var viewModel = (GetLiveVacanciesApiResponse.LiveVacancy)result.LiveVacancy;
            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error invoking Get Live Vacancy");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Route("{vacancyReference}")]
    public async Task<IActionResult> PostSendClosingSoonNotifications([FromRoute] long vacancyReference, [FromQuery] int daysUntilClosing)
    {
        try
        {
            await mediator.Send(new ProcessApplicationReminderCommand
            {
                VacancyReference = vacancyReference,
                DaysUntilClosing = daysUntilClosing
            });
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending closing soon notifications");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost]
    [Route("{vacancyReference}/close")]
    public async Task<IActionResult> PostVacancyClosed([FromRoute, Required] long vacancyReference)
    {
        try
        {
            await mediator.Send(new ProcessVacancyClosedEarlyCommand(vacancyReference));
            return Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error closing vacancy with vacancyReference: {VacancyReference}", vacancyReference);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}