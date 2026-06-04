using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Commands.UpsertProviderCourseForecasts;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Queries.GetProviderCourseForecasts;

namespace SFA.DAS.RoatpCourseManagement.Api.Controllers;

[ApiController]
[Tags("Provider Courses")]
[Route("providers/{ukprn}/courses/{larsCode}/forecasts")]
public class ProviderCourseForecastsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProviderCourseForecasts([FromRoute] int ukprn, [FromRoute] string larsCode, CancellationToken cancellationToken)
    {
        GetProviderCourseForecastsQueryResult result = await _mediator.Send(new GetProviderCourseForecastsQuery(ukprn, larsCode), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpsertProviderCourseForecasts([FromRoute] int ukprn, [FromRoute] string larsCode, [FromBody] IEnumerable<UpsertProviderCourseForecastModel> forecasts, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpsertProviderCourseForecastsCommand(ukprn, larsCode, forecasts), cancellationToken);
        return NoContent();
    }
}
