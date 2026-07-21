using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Commands.UpsertProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Models;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[ApiController]
[Route("providers/{ukprn}/allowed-courses")]
public class ProviderAllowedCoursesController(IMediator _mediator, ILogger<ProviderAllowedCoursesController> _logger) : ControllerBase
{
    [HttpPost("{larsCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpsertProviderAllowedCourse([FromRoute] int ukprn, [FromRoute] string larsCode, [FromBody] UpsertProviderAllowedCourseModel request)
    {
        _logger.LogInformation("Request to upsert provider allowed course for UKPRN {Ukprn} and LarsCode {LarsCode}", ukprn, larsCode);

        var command = new UpsertProviderAllowedCourseCommand
        {
            Ukprn = ukprn,
            LarsCode = larsCode,
            UserId = request.UserId,
            UserDisplayName = request.UserDisplayName,
            LastDateStarts = request.LastDateStarts
        };

        await _mediator.Send(command);

        return NoContent();
    }
}
