using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CivilServiceVacanciesController(IMediator mediator,
    ILogger<CivilServiceVacanciesController> logger) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        logger.LogInformation("Get Civil Service Vacancies invoked");

        try
        {
            var result = await mediator.Send(new GetCivilServiceJobsQuery(), cancellationToken);
            var viewModel = (GetLiveVacanciesApiResponse)result;
            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred getting civil service vacancies");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}