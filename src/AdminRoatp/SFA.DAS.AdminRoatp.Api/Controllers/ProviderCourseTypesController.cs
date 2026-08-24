using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Commands.RestrictProvider;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.AdminRoatp.Api.Controllers;

[ApiController]
[Route("providers/{ukprn}/course-types")]
public class ProviderCourseTypesController(IMediator _mediator, ILogger<ProviderCourseTypesController> _logger) : ControllerBase
{
    [HttpPost("{courseType}/restrict")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RestrictProvider([FromRoute] int ukprn, [FromRoute] CourseType courseType, [FromBody] RestrictProviderModel request)
    {
        _logger.LogInformation("Request to restrict provider for UKPRN {Ukprn} and CourseType {CourseType}", ukprn, courseType);

        var command = new RestrictProviderCommand
        {
            Ukprn = ukprn,
            CourseType = courseType,
            UserId = request.UserId,
            UserDisplayName = request.UserDisplayName,
        };

        await _mediator.Send(command);

        return NoContent();
    }
}
