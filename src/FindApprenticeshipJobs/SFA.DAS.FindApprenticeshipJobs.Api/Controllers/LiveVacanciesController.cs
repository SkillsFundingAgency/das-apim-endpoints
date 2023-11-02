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

    public LiveVacanciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] uint pageSize, [FromQuery] uint pageNo, CancellationToken cancellationToken)
    {
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
}