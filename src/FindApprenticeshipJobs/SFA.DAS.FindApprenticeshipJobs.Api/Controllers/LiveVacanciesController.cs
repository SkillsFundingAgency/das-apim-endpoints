using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LiveVacanciesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LiveVacanciesController> _logger;

    public LiveVacanciesController(IMediator mediator, ILogger<LiveVacanciesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] uint pageSize, [FromQuery] uint pageNo, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get Live Vacancies invoked");

        try
        {
            var result = await _mediator.Send(new GetLiveVacanciesQuery { PageNumber = (int)pageNo, PageSize = (int)pageSize }, cancellationToken);
            var viewModel = (GetLiveVacanciesApiResponse)result;
            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet]
    [Route("{vacancyReference}")]
    public async Task<IActionResult> GetLiveVacancy([FromRoute] long vacancyReference, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Get Live Vacancy invoked - vacancy reference: {vacancyReference}");

        try
        {
            var result = await _mediator.Send(new GetLiveVacancyQuery { VacancyReference = vacancyReference }, cancellationToken);
            var viewModel = (GetLiveVacanciesApiResponse.LiveVacancy)result.LiveVacancy;
            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}