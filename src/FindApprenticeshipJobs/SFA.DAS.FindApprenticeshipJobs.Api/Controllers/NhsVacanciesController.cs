using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindApprenticeshipJobs.Api.Models;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries;

namespace SFA.DAS.FindApprenticeshipJobs.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class NhsVacanciesController(IMediator mediator, ILogger<NhsVacanciesController> logger) : ControllerBase
{
    
    [HttpGet]
    public async Task<IActionResult> Get( CancellationToken cancellationToken)
    {
        logger.LogInformation("Get Live Vacancies invoked");

        try
        {
            var result = await mediator.Send(new GetNhsJobsQuery {}, cancellationToken);
            var viewModel = (GetLiveVacanciesApiResponse)result;
            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred getting live vacancies");
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }
}