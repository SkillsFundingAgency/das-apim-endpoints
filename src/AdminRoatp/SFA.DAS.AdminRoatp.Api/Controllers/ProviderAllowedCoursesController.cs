using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Commands.AddProviderAllowedCourse;
using SFA.DAS.AdminRoatp.Application.Commands.PatchProviderAllowedCourse;
using SFA.DAS.AdminRoatp.Application.Queries.GetProviderAllowedCourses;
using SFA.DAS.AdminRoatp.Infrastructure;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[ApiController]
[Route("providers/{ukprn}/allowed-courses")]
public class ProviderAllowedCoursesController(IMediator _mediator, ILogger<ProviderAllowedCoursesController> _logger) : ControllerBase
{
    [HttpPost("{larsCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddProviderAllowedCourse([FromRoute] int ukprn, [FromRoute] string larsCode, [FromBody] AddProviderAllowedCourseModel request)
    {
        _logger.LogInformation("Request to add provider allowed course for UKPRN {Ukprn} and LarsCode {LarsCode}", ukprn, larsCode);

        var command = new AddProviderAllowedCourseCommand
        {
            Ukprn = ukprn,
            LarsCode = larsCode,
            UserId = request.UserId,
            UserDisplayName = request.UserDisplayName,
            LastDateStarts = request.LastDateStarts,
            IsStartRestricted = request.IsStartRestricted,
        };

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpPatch("{larsCode}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> PatchProviderAllowedCourse([FromRoute] int ukprn, [FromRoute] string larsCode, [FromBody] PatchProviderAllowedCourseRequestModel request, [FromHeader(Name = Constants.RequestingUserIdHeader)] string userId, [FromHeader(Name = Constants.RequestingUserNameHeader)] string userName)
    {
        _logger.LogInformation("Request to patch provider allowed course for UKPRN {Ukprn} and LarsCode {LarsCode}", ukprn, larsCode);

        var command = new PatchProviderAllowedCourseCommand
        {
            UserId = userId,
            UserDisplayName = userName,
            Ukprn = ukprn,
            LarsCode = larsCode,
            LastDateStarts = request.LastDateStarts
        };

        await _mediator.Send(command);

        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(GetProviderAllowedCoursesResponse))]
    public async Task<IActionResult> GetProviderAllowedCourses([FromRoute] int ukprn, [FromQuery] CourseType? courseType = null, CancellationToken cancellationToken = default)
    {
        GetProviderAllowedCoursesQuery query = new(ukprn, courseType);
        GetProviderAllowedCoursesResponse result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
