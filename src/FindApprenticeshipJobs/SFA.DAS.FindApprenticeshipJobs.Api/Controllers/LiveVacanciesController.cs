using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        var liveVacancies = await _mediator.Send(new GetLiveVacanciesQuery { PageNumber = (int)pageNo, PageSize = (int) pageSize }, cancellationToken);

        if (liveVacancies == null)
        {
            return NotFound();
        }

        return Ok(liveVacancies);
    }
}