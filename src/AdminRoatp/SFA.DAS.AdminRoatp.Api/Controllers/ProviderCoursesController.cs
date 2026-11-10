using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderCourse;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[ApiController]
[Route("providers/{ukprn}")]
[Tags("Provider Courses")]
public class ProviderCoursesController(IMediator _mediator, ILogger<ProviderCoursesController> _logger) : ControllerBase
{
    [HttpGet("courses/{larsCode}")]
    [ProducesResponseType(typeof(GetProviderCourseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProviderCourse([FromRoute] int ukprn, [FromRoute] string larsCode, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Request received to get provider course for UKPRN {Ukprn} and LarsCode {LarsCode}", ukprn, larsCode);

        GetProviderCourseQuery query = new(ukprn, larsCode);
        GetProviderCourseResponse? result = await _mediator.Send(query, cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }
}
