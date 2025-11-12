using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminRoatp.Application.Commands.DeleteOrganisationShortCourseTypes;
using SFA.DAS.AdminRoatp.Application.Commands.UpdateOrganisationCourseTypes;
using SFA.DAS.AdminRoatp.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using System.Net;

namespace SFA.DAS.AdminRoatp.Api.Controllers;
[Route("organisations")]
[ApiController]
public class OrganisationCourseTypesController(IMediator mediator, ILogger<OrganisationCourseTypesController> _logger) : ControllerBase
{
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(IDictionary<string, string>))]
    [Route("{ukprn}/course-types")]

    public async Task<IActionResult> UpdateCourseTypes([FromRoute] int ukprn, [FromBody] UpdateCourseTypesModel model, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to update course types for Ukprn: {Ukprn}", ukprn);
        UpdateOrganisationCourseTypesCommand command = new(ukprn, model.CourseTypeIds, model.UserId);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    [Route("{ukprn}/short-courses")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]

    public async Task<IActionResult> DeleteShortCourseTypes([FromRoute] int ukprn, [FromHeader(Name = Constants.RequestingUserIdHeader)] string userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to DeleteShortCourseTypes for Ukprn: {Ukprn}", ukprn);
        DeleteOrganisationShortCourseTypesCommand command = new(ukprn, userId);
        HttpStatusCode response = await mediator.Send(command, cancellationToken);
        return new StatusCodeResult((int)response);
    }
}